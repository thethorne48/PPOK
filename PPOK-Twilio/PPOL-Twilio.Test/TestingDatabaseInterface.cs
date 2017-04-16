using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Dapper;
using PPOK.Domain.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PPOL_Twilio.Test
{
    public class TestingDatabaseInterface : IDatabaseInterface
    {
        private struct ExpectedQuery
        {
            public string sql;
            public object args;
            public IEnumerable<dynamic> result;
        }

        private Queue<ExpectedQuery> queries = new Queue<ExpectedQuery>();

        public void Expect(string sql, object args, IEnumerable<dynamic> result = null)
        {
            queries.Enqueue(new ExpectedQuery { sql = sql, args = args, result = result });
        }

        public void AssertArgs(object expected, object paras)
        {
            var expectedKeys = expected.GetType().GetProperties().Select(p => p.Name);
            var paraKeys = new HashSet<string>(
                paras is DynamicParameters
                ? ((DynamicParameters)paras).ParameterNames
                : paras.GetType().GetProperties().Select(p => p.Name)
            );
            foreach(var key in expectedKeys)
            {
                Assert.IsTrue(paraKeys.Contains(key));
                var expectedValue = expected.GetType().GetProperty(key).GetValue(expected);
                var paraValue = (
                    paras is DynamicParameters
                    ? ((DynamicParameters)paras).Get<object>(key)
                    : paras.GetType().GetProperty(key).GetValue(paras)
                );
                Assert.AreEqual(expectedValue, paraValue);
            }
        }

        public void Dispose() { }

        public void Execute(string sql, object args = null)
        {
            Assert.AreNotEqual(0, queries.Count, "Execute: " + sql);
            ExpectedQuery query = queries.Dequeue();
            Assert.AreEqual(query.sql, sql);
            AssertArgs(query.args, args);
            Assert.IsNull(query.result);
        }

        public IEnumerable<dynamic> Query(string sql, object args = null)
        {
            Assert.AreNotEqual(0, queries.Count, "Query: " + sql);
            ExpectedQuery query = queries.Dequeue();
            Assert.AreEqual(query.sql, sql);
            AssertArgs(query.args, args);
            return query.result;
        }
    }
}
