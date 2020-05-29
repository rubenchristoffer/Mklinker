# Contributing

All contributions are appreciated and anyone can contribute, no matter your experience level! Do make sure you read through the workflow below and the general guidelines to get started.  

If you have any questions, don't hesitate to [create a new issue](https://github.com/rubenchristoffer/Mklinker/issues/new) with the label _question_.

## Workflow
Start by following the instructions for [Setting up project](#settingup). After that you can find an issue that you would like to work on and leave a comment indicating that you would like to work on the issue / feature. You will then get assigned the issue if nobody else is working on it. You can also create an issue yourself if you have any questions or if you have a new feature that you would like to suggest.

When you have made your changes, read the section below regarding [pushing changes to Github](#push)

_**NOTE:** Getting assigned an issue isn't a strict requirement, but will increase the chance of getting pull request accepted and helps to keep track of what is being worked on._

## Setting up project <a name="settingup"></a>

Start by forking the project on Github by clicking the "Fork" button. Then, clone the forked repository by running the following command (make sure to replace [USERNAME] with your username):

    git clone git@github.com:[USERNAME]/Mklinker.git

A Visual Studio 2019 project file (Mklinker.sln) is already included in the repository, so just open this in Visual Studio 2019 if that is the IDE you want to use. It is of course possible to use a different IDE, but Visual Studio 2019 is recommended.

Make sure the tests pass by running `publish.bat` or `publish.sh` (you need to have [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1) installed). It is also possible to run the tests inside Visual Studio 2019 using the Test Explorer.

## Pushing changes to Github <a name="push"></a>
Push to your fork and [submit a pull request](https://github.com/rubenchristoffer/Mklinker/compare/dev...) if the tests do not fail. Make sure that you set the base branch to _dev_. You might have to click on `compare across forks` to select your fork as compare branch.

After creating pull request, all you have to do is wait until it is either approved or you get comments. Thanks for your contribution!

## General guidelines <a name="guidelines"></a>
Here's a list of things that will increase the chance of your pull request being accepted:

* Write unit tests (refer to other tests in codebase to get inspiration)
* Try to follow code style to the best of your ability (Visual Studio 2019 settings can be found [here](https://gist.github.com/rubenchristoffer/2613f1af2cd6c458c6593674b29bd81f))
* Write good commit messages that explains the changes

_**NOTE:** It's not the end of the world if you miss some of these guidelines, but following them will lead to higher chance of getting pull request approved_
