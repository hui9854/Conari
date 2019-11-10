# [Conari](https://github.com/3F/Conari)

[![](https://raw.githubusercontent.com/3F/Conari/master/Conari/Resources/Conari_v1.png)](https://github.com/3F/Conari)

Conari engine represents powerful platform for work with unmanaged memory, pe-modules, and their data: Libraries, Executable Modules, other native (C/C++, ...) and raw binary data.

Lightweight and flexible binding, even accessing to complex types like structures without their declaration at all. Also contains wrappers for types like unmanaged structures, unmanaged strings, and much more.

[![Build status](https://ci.appveyor.com/api/projects/status/qc1d3ofsso8fd67t/branch/master?svg=true)](https://ci.appveyor.com/project/3Fs/conari/branch/master)
[![release-src](https://img.shields.io/github/release/3F/Conari.svg)](https://github.com/3F/Conari/releases/latest)
[![License](https://img.shields.io/badge/License-MIT-74A5C2.svg)](https://github.com/3F/Conari/blob/master/LICENSE)
[![NuGet package](https://img.shields.io/nuget/v/Conari.svg)](https://www.nuget.org/packages/Conari/) 
[![Tests](https://img.shields.io/appveyor/tests/3Fs/conari/master.svg)](https://ci.appveyor.com/project/3Fs/conari/build/tests)

[![Build history](https://buildstats.info/appveyor/chart/3Fs/conari?buildCount=10&includeBuildsFromPullRequest=true&showStats=true)](https://ci.appveyor.com/project/3Fs/conari/history)

> 1:[ ***[Quick start](https://github.com/3F/Conari/wiki/Quick-start)*** ] 2:[ [Basic examples for C++ and C#](https://www.youtube.com/watch?v=9Hyg3_WE9Ks) ] 3:[ [Complex types and Strings](https://www.youtube.com/watch?v=QXMj9-8XJnY) ]
> -> { **[Wiki](https://github.com/3F/Conari/wiki)** }


[![](https://raw.githubusercontent.com/3F/Conari/master/Conari/Resources/screencast_Complex_types.jpg)](https://www.youtube.com/watch?v=QXMj9-8XJnY)


## License

The [MIT License (MIT)](https://github.com/3F/Conari/blob/master/LICENSE)

```
Copyright (c) 2013-2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
```

[ [ ☕ Donate ](https://3F.github.com/Donation/) ]

Conari contributors: https://github.com/3F/Conari/graphs/contributors

We're waiting for your awesome contributions!


## Why Conari ?


**Easy to start:**

```csharp
using(var l = new ConariL("...")) {
    // ...
}
```

**Dynamic features** (**DLR**, *fully automatic way*) when using of *unmanaged* code:

```csharp
var ptr     = d.test<IntPtr>(); //lambda ~ bind<Func<IntPtr>>("test")();
var codec   = d.avcodec_find_encoder<IntPtr>(AV_CODEC_ID_MP2); //lambda ~ bind<Func<ulong, IntPtr>>("avcodec_find_encoder")(AV_CODEC_ID_MP2);
              d.push(); //lambda ~ bind<Action>("push")();
              d.create<int>(ref cid, out data); //lambda ~ bind<MyFunc<Guid, object>>("create")(ref cid, out data);
```

It does not require the any configuration from you, because Conari will do it **automatically**. *Works perfectly for most popular libraries like: Lua, 7-zip, FFmpeg, ...*

Custom **Lambda expressions** (*semi-automatic way*) when using of *unmanaged* code:

```csharp
using(var l = new ConariL("Library.dll"))
{
    l.bind<Action<int, int>>("call")(2, 1); 
    double num = l.bind<Func<IntPtr, int, double>>("tonumber")(L, 4);
}
```

This also does not require the creation of any additional delegates. Just use `bind<>` methods with additional types and have fun!

```csharp
l.bind<...>("function")
```

```csharp
// you already may invoke it immediately as above:
l.bind<Action<int, string>>("set")(-1, "Hello from Conari !");

// or later:
var set = l.bind<Action<int, string>>("set");
...
set(-1, "Hello from Conari !");
```

**Lazy loading:**

```csharp
using(var l = new ConariL(
                    new Config("Library.dll") {
                        LazyLoading = true
                    }))
{
    ...
}
```

**Native C/C++ structures without declaration** **[[?](https://github.com/3F/Conari/issues/2)]**:

```csharp

// IMAGE_FILE_HEADER: https://msdn.microsoft.com/en-us/library/windows/desktop/ms680313.aspx
dynamic ifh = NativeData
                ._(data)
                .t<WORD, WORD>(null, "NumberOfSections")
                .align<DWORD>(3)
                .t<WORD, WORD>("SizeOfOptionalHeader")
                .Raw.Type;
                
if(ifh.SizeOfOptionalHeader == 0xE0) { // IMAGE_OPTIONAL_HEADER32
    ... 
}

// IMAGE_DATA_DIRECTORY: https://msdn.microsoft.com/en-us/library/windows/desktop/ms680305.aspx
dynamic idd = (new NativeData(data))
                    .t<DWORD>("VirtualAddress") // idd.VirtualAddress
                    .t<DWORD>("Size")           // idd.Size
                    .Raw.Type;
```

```csharp
IntPtr ptr ...
Raw mt = ptr.Native()
                .align<int>(2, "a", "b")
                .t<IntPtr>("name")
                .Raw;
            
-     {byte[0x0000000c]} byte[]
        [0]    0x05    byte --
        [1]    0x00    byte   |
        [2]    0x00    byte   |
        [3]    0x00    byte --^ a = 5
        [4]    0x07    byte --
        [5]    0x00    byte   |
        [6]    0x00    byte   |
        [7]    0x00    byte --^ b = 7
        [8]    0x20    byte --
        [9]    0x78    byte   |_ pointer to allocated string: (CharPtr)name
        [10]   0xf0    byte   |
        [11]   0x56    byte --
...
```

**Calling Convention** & **Name-Decoration** **[[?](https://github.com/3F/Conari/issues/3)]**:

```csharp
using(var l = new ConariL("Library.dll", CallingConvention.StdCall))
{
    //...
    l.Mangling = true; // _get_SevenStdCall@0 <-> get_SevenStdCall
    l.Convention = CallingConvention.Cdecl;
}
```

**Exported Variables & Raw access [[?](https://github.com/3F/Conari/issues/7#issuecomment-269123650)]:**

```csharp
// v1.3+
l.ExVar.DLR.ADDR_SPEC // 0x00001CE8
l.ExVar.get<UInt32>("ADDR_SPEC"); // 0x00001CE8
l.ExVar.getField(typeof(UInt32).NativeSize(), "ADDR_SPEC"); // Native.Core.Field via raw size
l.Svc.native("lpProcName"); // Raw access via NativeData & Native.Core !
//v1.0+: Use Provider or ConariL frontend via your custom wrapper.
```

**Aliases for exported-functions and variables [[?](https://github.com/3F/Conari/issues/9#issuecomment-273855381)]:**

```csharp
// v1.3+
l.Aliases["Flag"] = l.Aliases["getFlag"] = l.Aliases["xFunc"]; //Flag() -> getFlag() -> xFunc()->...
// ...
l.DLR.getFlag<bool>();
```

**Additional types:**

* BSTR, CharPtr, WCharPtr, float_t, int_t, ptrdiff_t, size_t, uint_t
* UnmanagedString - allocation of the new unmanaged strings.
* ...

```csharp
size_t len;
CharPtr name = c.bind<FuncOut3<int, size_t, IntPtr>>("to")(1, out len);
string myName += name; // (IntPtr)name; .Raw; .Ansi; .Utf8; ...
```

**Events**:

```csharp
l.ConventionChanged += (object sender, DataArgs<CallingConvention> e) =>
{
    DLR = newDLR(e.Data);
    LSender.Send(sender, $"DLR has been updated with new CallingConvention: {e.Data}", Message.Level.Info);
};

l.BeforeUnload += (object sender, DataArgs<Link> e) =>
{
    // Do not forget to do something before unloading a library
};

...
```


and more !


### [Examples](https://github.com/3F/Conari/wiki/Examples)

* *[List of real usage via Conari engine](https://github.com/3F/Conari/wiki/Projects)*

#### Sample for DLR
How about to use [regXwild](https://github.com/3F/regXwild) (Fast and powerful wildcards on native unmanaged C++) in your C# code ? It's easy:

```csharp
using(var l = new ConariL("regXwild.dll")) {
...
    if(l.DLR.searchEssC<bool>((WCharPtr)data, (WCharPtr)filter, false)) {
        // ...
    }
}
```
yes, you don't need to do anything else! Conari will prepare all required operations and binding with native method instead of you:

```cpp
REGXWILD_API bool searchEssC(const TCHAR* data, const TCHAR* filter, bool ignoreCase);
```

have fun!


## How to get Conari

* NuGet: [![NuGet package](https://img.shields.io/nuget/v/Conari.svg)](https://www.nuget.org/packages/Conari/)
* [GetNuTool](https://github.com/3F/GetNuTool): `msbuild gnt.core /p:ngpackages="Conari"` or **[gnt](https://3f.github.io/GetNuTool/releases/latest/gnt/)** /p:ngpackages="Conari"
* [GitHub Releases](https://github.com/3F/Conari/releases) [ [latest](https://github.com/3F/Conari/releases/latest) ]
* CI builds: [`CI /artifacts`](https://ci.appveyor.com/project/3Fs/conari/history) or find `🎲 CI build` on [GitHub Releases](https://github.com/3F/Conari/releases) page.
