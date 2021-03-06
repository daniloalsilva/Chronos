# Chronos

When you develop plugins to monitoring your Windows enviroment, in some cases it is impossible to cache the plugin execution.

But, what you can do if some plugin cannot be executed twice in same hour, or even in the same day? Still, you cannot discard this specific monitoring plugin?

Chronos is a software used as a cache layer to store the last result from a software/plugin/script execution for a specified time.

If execution time exceeded defined 'timeout', Chronos will terminate plugin execution.

Stored cache data will be allocated on current execution directory.

### Example:

    C:\>SomeApp\Chronos.exe [params]
       ^
       └─── cache will be created in C:\Cache
 ----
    C:\SomeApp>Chronos.exe [params]
              ^
              └─── cache will be created in C:\SomeApp\Cache

### Executing scripts

Chronos works fine with executables (.exe files), but in case you have to execute scripts (such as `powershell`), you can omit initial parameters, (such as `"powershell.exe -file"`)

**Extension/Executor mapping:**

| Extension | Executor                               | Intial Params |
|-----------|----------------------------------------|---------------|
| .ps1      | \WindowsPowerShell\v1.0\powershell.exe | -File         |
| .vbs      | \cscript.exe                           | //NoLogo      |
| .wsf      | \WScript.exe                           |               |

> the "Executor" will always use "Environment.SystemDirectory" as path suffix

### Params explanation:

    Chronos.exe 30 10 C:\Windows\System32\cmd.exe /c dir
                ^  ^  ^                           ^    ^
                │  |  └ plugin             plugin └──┬─┘
                │  └─── cache time         params ───┘
                └────── timeout
 ----
    Chronos.exe clearCache
                ^        ^
                └────┬───┘
                     └─── clear all Chronos cache

### Using examples:


Timeout: 60 seconds, keepping cache: 5 minutes

    Chronos.exe 60 5 C:\monitoring\check_http.ps1 http://www.google.com
    
Timeout: 30 seconds, keepping cache: 6 hours

    Chronos.exe 30 360 C:\monitoring\check_uptime.vbs 50 60

Clear chronos cache:

    Chronos.exe clearCache

> Obs.: Take some atention at arch used (x86, x64) when compiling Chronos.
> Chronos will use the same compiled architecture to execute plugins, so if you want to call "powershell" as x64, you MUST compile chronos as x64
> Some plugins doesn't work without correct architecture, you can use Chronos to trick Operating System and force execution in a choosen arch.
