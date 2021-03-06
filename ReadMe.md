#CmdrX.exe

CmdrX is a command 'runner'. This c# windows application is designed to execute multiple console commands.

## Command Execution
The commands are executed in sequence as specified in a
user configued Xml configuration file.  CmdrX will execute
any legitimate operating system command.

## Commands Configured In Xml File
When activated, CmdrX will search the current directory
for the default Xml configuration file, **CmdrXCmds.xml**.
Users can modify **CmdrXCmds.xml** to execute any desired
series of commands.

## Example Usage
This application can be used to setup new projects with component
loaders like  npm, jspm, and bower. This completely automates,
and documents, the setup of new javascript projects.

## Xml Parameter Conversion
If an Xml command argument contains the string %(CURDATESTR)%, CmdrX
will convert this parameter to a current
date string (yyyMMddHHmmss).

Eligible Xml elements are labeled in the xml command file as follows:
* DefaultConsoleCommandExecutor
* ConsoleCommandExeArguments
* ExecutableTarget
* CommandToExecute
* CommandModifier
* CommandArguments
