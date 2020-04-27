# WCA.IntegrationTests
This project is for integration tests that call out to 3rd party services such as **Actionstep**, or **InfoTrack**.

This project is configured to use the same database `WCADb` as the `WCA.Web` project. This is important as it relies on valid credentials in that database.

## IMPORTANT
It is important that tests do not write to `WCADb`.