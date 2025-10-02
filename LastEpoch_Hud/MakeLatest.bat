@echo off
SET "keyboard_file=LastEpoch_Hud(Keyboard).rar"
SET "gamepad_file=LastEpoch_Hud(WinGamepad).rar"
SET "unwanted_file=LastEpoch_Hud.deps.json"
cd %~dp0
cd ..\Build\Keyboard\net6.0\
SET "keyboard_dir=%cd%
cd %~dp0
cd ..\Build\WinGamepad\net6.0\
SET "gamepad_dir=%cd%
IF EXIST %keyboard_dir%\%unwanted_file% (
		del "%keyboard_dir%\%unwanted_file%"
)
IF EXIST %gamepad_dir%\%unwanted_file% (
		del "%gamepad_dir%\%unwanted_file%"
)
cd %~dp0
cd ..\Latest\
SET "latest_dir=%cd%
mkdir %latest_dir%
IF EXIST %latest_dir%\%keyboard_file% (
	del "%latest_dir%\%keyboard_file%"
)
IF EXIST %latest_dir%\%gamepad_file% (
	del "%latest_dir%\%gamepad_file%"
)
xcopy "%keyboard_dir%" "F:\SteamLibrary\steamapps\common\Last Epoch\Mods" /E /Y
