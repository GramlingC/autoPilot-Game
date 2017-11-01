# autoPilot-Project-Folder

Please note: These are just some generic guidelines to workflow, remember to communicate with your team as you work to prevent any issues or if you have any questions.

# Workflow
[Checkout this interactive guide](https://guides.github.com/introduction/flow/) to get an idea of the workflow, nevermind the details.

## Identify the Feature
Identify the feature that you want to begin working on.

## Pull master updates
Pull the latest updates on your local repo, make sure you keep ``master`` current.
```
git checkout master
git pull origin master
```

## Create a Feature Branch
Make sure you are on master, we want to branch from a stable state.
```
git checkout master
```
Create a _Feature Branch_ with a well formatted branch name:
```
git checkout -b shortFeatureName
```
Then push it to the remote, so we know you're working on it
```
git push origin shortFeatureName
```

##Work on the Feature Branch
Write your code and commit often (anytime you change something or write something significant)
```
git add -A
git commit -m 'Useful commit message'
```

##Keep Feature Branch up-to-date with master
As you work periodically, after you know of changes to master or when your feature is complete -

Use git pull to pull the changes made to master and merge them into your current working branch:
```
git pull origin master
```
Alternatively, fetch the remote master and rebase your _Feature Branch_ to those changes: 
```
git fetch origin master
git rebase origin/master
```
Either way, there may be merge conflicts that arise.  If this happens you will need to resolve the conflicts:
```
git mergetool
```
If you don't want to have to deal with this, message in the programming slack to let us known you need help resolving a merge conflict.

##Push Your Feature Branch
After some commits, it may be a good idea to upload your code to the online repo, so everyone else can see what you've worked on:
``` git push origin shortFeatureName```

##Create a Pull Request
When you've completed your changes, you'll want to submit a Pull Request to let us know it's ready to be merged.

On GitHub, navigate to your feature branch and create a Pull Request to the ``testing`` branch.

[See this guide for more detailed instructions](http://yangsu.github.io/pull-request-tutorial)

##Peer Review Pull Request
Review code and suggest fixes, each member must approve the code before approving.
* Fixes would be made locally on the feature branch and then the updates pushed with
``` 
git push origin shortFeatureName
```
* This will automatically update the pull request

##Approve Pull Request
After each team member has approved your feature we will pull your code to the ```master``` branch for integration testing. As better tests are developed, this process will be streamlined, but for now we will have one person incharge of integrating changes.

##Delete Feature Branch (Optional)
You can do this immediately after it is merged into master to keep the repo tidy, but you may also choose to wait until later
* Locally - Using ``git branch -d shortFeatureName``
* On GitHub - Using the option on the _Pull Request_

#Code and Style

##Style
A good C++ style to refer to: [Google C++ style](https://google-styleguide.googlecode.com/svn/trunk/cppguide.html)
