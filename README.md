
# Code Structure

- The project is setup using dotnet core classlib, and nunit project templates.
- The namespace suggested for the project is `FML.Services.ParcelCostService` (thus the tests project is `FML.Services.ParcelCostService.Tests` under the same solution)
- The development environment used a mixture of VSCode and Visual Studio.

# Current State

- Build passes.
- 16 unit tests pass:
  ![image](https://github.com/rikkigouda/pcs/assets/10061151/c8a751f9-2002-4834-a390-3d5bdfd79d9d)

# Assumtions made
- There has been a number of assumptions I have made throughout the code - all detailed via compiler REVIEW/TODO flags.
- The requirements raised few questions too which I hoped to raise in the presentation meeting.

# Improvements that I would do next
- Dependency Injection and extending the design into a more encapsulated set of services, specially around orchestration of the processing/pricing rules.
- The design currently includes a fair bit of hard-code. I am more than positive there are lots of bugs that the tests are not covering. Thus the next immediate step I would pick is to increase the code coverage and add more business scenarios to the existing (initial) set of test cases.
- Cleanup of design and naming of certain services. Ex: "Processing" is very generic, I would want to spend more time and structure the terminologies used to improve the coherence of the design and the code.

