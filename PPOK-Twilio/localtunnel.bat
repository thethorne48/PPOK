@echo off
if not exist "%appdata%/npm" (
	echo Need to install npm to continue (this project requires localtunnel)
) else (
	if not exist "%appdata%/npm/lt" (
		echo Installing localtunnel for Twilio...
		npm install -g localtunnel
	)

	"%appdata%/npm/lt" --port 51335 --subdomain ppoktwilio
)
pause