# Cornojobless Certificate Manager
<img src="cornojobless.png" align="right"/>

**Cornojobless\* Certificate Manager** is a Windows desktop application built with C# and WPF, designed to help you clean up certificate stores bloated with unused/expired certificates and whatnot.

> This project is under development. Some features (like deletion auditing) are not implemented yet but they’re coming

## Features
- **Certificate backup**
  - Export certificates before deletion (stored in `Documents/CertificateBackups`)
  - Works with **exportable certificates**

- **Certificate deletion**
  - Supported Windows certificate stores:
    - `CurrentUser`
    - `LocalMachine`

- **(not so) Advanced filtering**
  - Filter certificates by:
    - **Issuer**\*
    - **Expired certificates**
    - **Exportable certificates**

- **Configurable issuers via `appsettings.json`**
  - Issuer list can be defined directly in configuration
  - Loaded dynamically at runtime

> \*Thumbprint filtering will also be supported in the near future :)
## How do I run it?

### Prerequisites

- Windows
- .NET SDK 8
- Visual Studio (recommended)

### Running it

1. git clone https://github.com/elcanion/CornojoblessCertificateManager.git
2. Open the solution in Visual Studio
3. Review and adjust `appsettings.json` (optional)
4. Build and run

> Managing certificates in `LocalMachine` may require running it with elevated privileges, so adjust your `app.manifest` requestedExecutionLevel as needed

## Important notes (read this before nuking your certs)

- Certificate deletion is **dangerous**.
- Always double-check your filters.
- Backups are your best friend, look for them in `Documents/CertificateBackups`.

## DISCLAIMER (please read this as well)
Let me be VERY clear:

- This tool **deletes Windows certificates**.
- **Things will break** if you delete the wrong certificate.
- If production goes down, **that’s on you**.
- Look at `Documents/CertificateBackups` if you *accidentally* delete something.
- **Do not blame this tool** if your boss asks why TLS or anything else stopped working. Blame **yourself** instead.

This application is provided **AS IS**, with **NO WARRANTIES**, **NO SAFETY NET**, and **ZERO RESPONSIBILITY** taken by me. Again, that’s on you.

So, you are fully responsible for:

- Verifying filters before executing actions.
- Understanding the impact of certificate deletion.
- Knowing that `Documents/CertificateBackups` may be your very last salvation.
- Running this in production environments. (I mean, really?)

If you are not 100% sure what a certificate does: **DO. NOT. DELETE. IT.**

And finally, by using this software, you agree that:

- At least you know what you’re doing.
- You will not blame me, this tool, your astrological sign, or even worse, your dog.
- You had plenty of warnings and ignored none of them.

> \*Its name is a dumb pun on "cronjob". For the unaware, "cornojob" roughly means, in Portuguese, an insufferable task (by the way, "corno" means "horn", hence the broken-horned gnu icon).
