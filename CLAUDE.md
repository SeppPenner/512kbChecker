# Project rules for Claude

## What this is

512kbChecker is a small Windows Forms desktop application. The user picks a folder, the program
searches it recursively for `*.gif` files and lists every file that is 512000 bytes or larger,
because such files are too large for IBM Lotus Notes Sametime. The UI is bilingual (German,
English) and switchable at runtime through a combo box. Distribution happens as an Inno Setup
installer, not as a NuGet package.

One solution `src/512kBChecker.sln` with exactly one project,
`src/512kBChecker/512kBChecker.csproj`. There is no test project, no example project and no second
project of any kind. Note the spelling: the repository folder is `512kbChecker` (small `b`), the
solution, project, assembly and installer are `512kBChecker` (capital `B`), and the namespace is
`_512kBChecker` with a leading underscore, because a C# identifier cannot start with a digit. All
three spellings are intentional, do not "unify" them.

Layout inside `src/512kBChecker`:

- `Program.cs`: entry point, `[STAThread]`, `Application.Run(new Main())`.
- `Main.cs`: the whole application logic. Folder dialog, recursive `*.gif` enumeration, size check,
  language handling.
- `Main.Designer.cs` plus `Main.resx`: Windows Forms designer output. `Main.resx` is about 330 kB
  because the window icon is embedded in it. Designer code is generated, it does not follow the
  hand written conventions below, do not reformat it by hand.
- `GlobalUsings.cs`: all usings of the project.
- `languages/de-DE.xml` and `languages/en-US.xml`: the UI texts.
- `Gifs.ico`: application and installer icon. `License.txt`: shipped next to the executable.

Translation comes from the NuGet package
[HaemmerElectronics.SeppPenner.Language](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.Language/)
(assembly and namespace `Languages`, source in the sibling repository `CSharpLanguageManager`).
Its runtime contract is convention based and this project depends on it:

- `LanguageManager` loads every `*.xml` from a `languages` directory beside the executing assembly.
- Each file deserializes into `Identifier`, `Name` and `Words/Word/Key` plus `Value`. The
  identifier must be a culture name that `CultureInfo` understands (`de-DE`, `en-US`).
- `GetWord` returns `null` for an unknown key, it does not throw and it does not fall back to
  another language. A new UI text therefore has to be added to **both** language files, otherwise
  one language silently shows an empty string.
- The two XML files are copied to the output directory with `CopyToOutputDirectory=Always`, the
  same holds for `License.txt`. Removing that is what makes the shipped program start without any
  texts.

## Build

```powershell
dotnet build src/512kBChecker.sln
```

- Single target framework `net10.0-windows`, `WinExe`, `UseWindowsForms`, `RuntimeIdentifiers`
  `win-x64`. This is a Windows only application, unlike the library it references.
- All build properties live directly in `src/512kBChecker/512kBChecker.csproj`. There is no
  `Directory.Build.props` in this repository.
- `TreatWarningsAsErrors` is enabled, so every warning breaks the build, NuGet warnings (`NU****`)
  from restore included. A clean build reports zero warnings, keep it that way.
- `NU1803` (HTTP source usage during restore) is the one warning suppressed via `NoWarn`. Fix
  warnings instead of extending that list. `NuGetAudit` and `NuGetAuditMode=all` are on, so a
  vulnerable transitive package fails the build too.
- Versions come from GitVersion.MsBuild out of the git tags, for example `1.0.10-1` for the first
  commit after tag `1.0.9`. Never edit a version property or an assembly version by hand.
- Restore needs nuget.org. If a private feed is configured globally on the machine and answers 404
  for public packages, restore fails with `NU1301`. Then build with an explicit source:
  `dotnet build src/512kBChecker.sln --source https://api.nuget.org/v3/index.json`.
- `Setup/build-setup-files.bat` deletes all `bin` and `obj` folders below `src`, then runs
  `dotnet publish -c Release -r win-x64 --self-contained true -o bin/publish` and removes the
  `*.pdb` files from the publish output. The installer ships the whole runtime, so the target
  machine needs no .NET installation, at the price of roughly 120 MB of publish output. The batch
  file does **not** run the Inno Setup compiler, that is a separate manual step.
- **There are no tests in this repository.** Never claim a test run happened. Verification means a
  clean build, and where behaviour changed, starting the built executable and pointing it at a
  folder with gif files.

## Code conventions

Follow the surrounding code, it is consistent throughout the hand written files:

- File header comment block with `<copyright file="..." company="Hämmer Electronics">` and a
  `<summary>`, then the file-scoped namespace `_512kBChecker`.
- XML doc comments on every type and every member, private members and event handlers included, no
  exceptions.
- `Nullable`, `ImplicitUsings` and `LangVersion latest` are enabled.
- New `using` directives go into `src/512kBChecker/GlobalUsings.cs`, inside the existing
  `#pragma warning disable IDE0065` block, never at the top of a file. The editorconfig requires
  usings inside the namespace (`csharp_using_directive_placement=inside_namespace:warning`), which
  global usings cannot satisfy, that is what the pragma is for. Do not add other pragmas. The
  comment text in that block is German because Visual Studio generated it, leave it alone.
- Fields, properties, methods and events are always accessed with `this.` qualification
  (`dotnet_style_qualification_for_*` at severity `warning`).
- `src/.editorconfig` also enforces braces everywhere, no multiple blank lines, four spaces, CRLF,
  UTF-8, file scoped namespaces and `IDE0005` as warning. Analyzer warnings are fixed, not
  silenced.
- `Main.cs` is deliberately split into small single purpose private methods (`Init`,
  `LoadTitleAndDescription`, `SearchFiles`, `CheckFiles`, `CheckFile`, ...). Keep new logic in that
  shape instead of growing one big handler.

## Known quirks

Do not silently "clean up" these, they are existing behaviour:

- The threshold is `>= 512000` bytes, decimal kB, not `512 * 1024 = 524288`. It matches the product
  name, changing it changes the result for every user.
- The button caption and the folder dialog description are set **only** in the `OnLanguageChanged`
  handler, the designer assigns no text. `Init` therefore has a required order:
  `InitializeLanguageManager` subscribes to the event, and only the following
  `LoadLanguagesToCombo` with `SelectedIndex = 0` fires it for the first time. Swapping those two
  calls leaves the button empty at startup.
- `SetCurrentLanguage("de-DE")` in `InitializeLanguageManager` runs before the event is subscribed,
  so it changes no caption. The language actually shown is whichever file ends up first in the
  combo box, which is `de-DE.xml` only because of the alphabetical file order.
- The window title is `Application.ProductName` plus `Application.ProductVersion`, and
  `ProductVersion` is the GitVersion informational version. On an untagged commit the title reads
  something like `512kBChecker 1.0.10-1+Branch.master.Sha.e3af4c2...`. Only a tagged build shows a
  clean version.
- `.gitignore` excludes `*.exe` and `[Bb]in`, yet `Setup/512kBChecker-Setup.exe` is tracked. It was
  added with `git add -f` and has to be updated the same way for every release.
- `Setup/512kBChecker-Setup.iss` is UTF-8 **with** BOM and has to stay that way. Inno Setup 6 reads
  a script as UTF-8 only when the BOM is there, without it the file is interpreted in the system
  ANSI codepage and `Hämmer Electronics` becomes `HÃ¤mmer Electronics` in the installer. Editors
  that save "UTF-8 without BOM" by default silently break this.
- `src/512kBChecker.sln` still declares only `Debug|x86` and `Release|x86` solution
  configurations mapped to `Any CPU`. It builds fine, do not "fix" it as a side task.

## Releasing

The tag comes **before** the installer build, never after. GitVersion derives the assembly version
from the tag, so an installer compiled on an untagged commit contains an executable that reports
something like `1.0.10-4+Branch.master.Sha...` in its window title instead of a clean `1.0.10`.

1. Make the change.
2. Add an entry at the top of `Changelog.md` in the existing format:
   `* **Version 1.0.10.0 (2026-08-10)** : Short description.`
3. Bump `MyAppVersion` in `Setup/512kBChecker-Setup.iss` to the same version, four parts.
4. Commit that, then tag the commit with the plain version number, no `v` prefix (`1.0.9`,
   `1.0.8`, ...).
5. Run `Setup/build-setup-files.bat`, it publishes the tagged commit to
   `src/512kBChecker/bin/publish`.
6. Compile `Setup/512kBChecker-Setup.iss` with Inno Setup, it writes
   `Setup/512kBChecker-Setup.exe`.
7. Commit that file with `git add -f`, then push the commits and the tag. This last commit sits
   after the tag, the same way `Updated setup.` sits after tag `1.0.9`.

Never run the publish or the installer build unless explicitly asked to release.

There is no CI configuration in this repository, no `.github` folder and no publish pipeline. The
AppVeyor badge in `README.md` points to a build that is configured outside of the repository. There
is no `Updating.md` and no `HowToUse.md` here, the `README.md` with the two screenshots
(`Screenshot_DE.PNG`, `Screenshot_EN.PNG`) is the only user documentation.

## Git

- **Never amend a commit.** No `git commit --amend`, not for a typo in the message, not to add a
  forgotten file, not even when the commit is still local. Write a follow-up commit instead. The
  release versions come from tags on exact commits, an amended commit leaves its tag pointing at a
  commit that no longer exists in the branch.

## Writing style

- Commit messages are written **in English only**: short, precise subject line, explanatory body
  when needed.
- Code comments and comments in project files such as `.csproj` are **always English**, regardless
  of the language used in the conversation.
- **No em dashes or en dashes** (`—`, `–`), neither in prose, commit messages, code comments nor
  documentation. Use a regular hyphen, comma, colon, parentheses or a separate sentence.
- German texts (documentation, chat replies, the `de-DE.xml` values) always use real umlauts and ß,
  never ASCII transliterations such as `ae`, `oe`, `ue` or `ss`. Identifiers, file names and
  configuration keys stay unchanged where umlauts are technically undesirable.
