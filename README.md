# TechnicalExcercise

This repository is home to Trainline API and associated projects.

## Requirements
- A git tool, like [SourceTree](https://www.sourcetreeapp.com/), to clone this repo.
- An IDE for .NET development: [Visual Studio](https://visualstudio.microsoft.com/downloads/) or [VSCode](https://code.visualstudio.com/download).
- The [.NET Core SDK (3.1+)](https://dotnet.microsoft.com/download) to build the application.

### Run the API
From [Visual Studio](https://visualstudio.microsoft.com/downloads/), you can directly run it and test it on Swagger, since no authorization is required.

### Run the Unit Testing
To test the Technical Excercise, run the tests on Trainline.ExchangeRate.Business.Tests and in TrainlineExchangeRates.Api.Tests.

### Unit Tests guidelines:
- Project on the same level of the project that has the method/s class/es to be tested. 
- i.e. Trainline.ExchangeRate.Business.Managers, added on the same level an xUnit project: Trainline.ExchangeRate.Business.Tests.
- When creating a new test project, Moq library can be added through nuget packages.
- It was used AAA arrangement for the tests (Arrange, Act, Assert) https://docs.microsoft.com/en-us/visualstudio/test/unit-test-basics?view=vs-2019.
- It was used SUT a.k.a. "System under test" https://en.wikipedia.org/wiki/System_under_test.
- Naming of test methods as Should_ExpectedBehavior_When_StateUnderTest (5th on): https://dzone.com/articles/7-popular-unit-test-naming.

### Extra things to do with more time
- Add some type of logging to check failures (maybe in the cloud).
- Change hardcoded values like "EUR" to an enum.
- Extract the validation method for the request from the Get endpoint.
- Instead of doing request.SourceCurrency == null || request.SourceCurrency == "" its preferred to make a string extension method (like IsNullOrEmpty()) to avoid repetition for common cases like this.
- Make logic for custom exceptions.
- When request Price is < 0, throw an specific Exception instead of a plain BadRequest.
- Make better initialization on the tests.
- Change the tests to use fluent assertions to have a more natural testing naming convention.
- Validate on the Manager the parameters also (although that was tested on the Controller), and add tests for that to the manager.
- Assess the precision on the transformed amount and make it configurable as needed.
- Assess the provider to see if it can provide a service of supported currencies, and eventually make a service to mantain that information if needed.
- Add HttpClientFactory in order to avoid hard-coding recurrent uris (and validate with the business it that could be a future scenario).


