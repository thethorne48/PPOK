(function () {
    var Prescription = {
        Patient: {
            ContactPreference: true,
            FirstName: true,
            LastName: true,
            Email: true,
            Phone: true,
            DOB: true,
            ZipCode: true
        }
    };

    ace.define('ace/mode/template_highlight_rules', ["require", "exports", "module", "ace/lib/oop", "ace/mode/text_highlight_rules"], function (require, exports, module) {
        "use strict";

        var oop = require("../lib/oop");
        var TextHighlightRules = require("./text_highlight_rules").TextHighlightRules;

        var TemplateHighlightRules = function () {

            var rules = {
                start: [
                    {
                        token: 'constant.char',
                        regex: '{{',
                        next: 'start'
                    },
                    {
                        token: 'keyword.open',
                        regex: '{',
                        next: 'Prescription'
                    }
                ],
                end: [
                    {
                        token: 'invalid',
                        regex: '[^}]+',
                        next: 'end'
                    },
                    {
                        token: 'keyword',
                        regex: '}',
                        next: 'start'
                    }
                ]
            };
            handle('Prescription', Prescription, '');

            function handle(name, props, path) {
                var arr = [];
                for(var key of Object.keys(props)) {
                    var value = props[key];
                    switch (typeof value) {
                        case 'boolean':
                            arr.push({
                                token: 'string'+path,
                                regex: key,
                                next: 'end'
                            });
                            break;
                        case 'object':
                            arr.push({
                                token: 'string' + path,
                                regex: key,
                                next: key + 'Dot'
                            });
                            handle(key, value, path+'.'+key);
                            break;
                    }
                }
                arr.push({
                    token: 'invalid' + path,
                    regex: '[^}]*',
                    next: 'end'
                });
                rules[name] = arr;
                rules[name + 'Dot'] = [{
                    token: 'operator'+path,
                    regex: '\\.',
                    next: name
                }, {
                    token: 'invalid' + path,
                    regex: '[^}]*}',
                    next: 'start'
                }];
            }
            console.log(rules);
            // regexp must not have capturing parentheses. Use (?:) instead.
            // regexps are ordered -> the first match is used
            this.$rules = rules;
        };

        oop.inherits(TemplateHighlightRules, TextHighlightRules);

        exports.TemplateHighlightRules = TemplateHighlightRules;
    });

    ace.define('ace/mode/recall-template',
        ["require", "exports", "module",
            "ace/lib/oop", "ace/mode/text", "ace/mode/template_highlight_rules",
            "ace/mode/template-auto-complete"
        ], function (require, exports, module) {
            "use strict";

            var oop = require("../lib/oop");
            var TextMode = require("./text").Mode;
            var HighlightRules = require("./template_highlight_rules").TemplateHighlightRules;
            require('ace/mode/template-auto-complete');

            var Mode = function () {
                this.HighlightRules = HighlightRules;
            };
            oop.inherits(Mode, TextMode);

            Mode.prototype.$id = 'ace/mode/template';

            exports.Mode = Mode;
        });

    ace.define('ace/mode/template-auto-complete', ['require', 'exports', 'module', 'ace/ext/language_tools'], function (require, exports, module) {
        "use strict";

        var langTools = require('../ext/language_tools');
        langTools.setCompleters([{
            getCompletions: function (editor, session, pos, prefix, callback) {
                var token = session.getTokenAt(pos.row, pos.column);
                token = token.type;

                var path = null;
                if (token.startsWith('string') || token.startsWith('invalid') || token.startsWith('operator')) {
                    path = token.split('.');
                    path.shift();
                } else
                if (token == 'keyword.open') {
                    path = [];
                }

                if (path) {
                    var obj = Prescription;
                    for(var part of path) {
                        obj = obj[part];
                    }

                    callback(null, Object.keys(obj).map(key => ({
                        value: key
                    })));
                } else {
                    callback(null, []);
                }
            }
        }]);
    });
})();