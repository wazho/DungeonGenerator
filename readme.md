## Getting Started

Clone or download in your Unity project folder **Assets**. Then extension will appear on the top of menu, it's called **Dungeon**.

You can see some examples in the menu.

```
> cd <your unity project, 'Editor' folder>
> git clone https://github.com/grass0916/DungeonGenerator.git
```

And check your task first from [Google sheet](https://docs.google.com/spreadsheets/d/1A1DMKRhcZCAshqFJlascTw-KpFhdhQTCEFTGTjbah5s/edit?usp=sharing).

---

## Paths

### views

Here is all layouts of editor window. `MenuMethods.cs` is the config for each windows.

### models

*Rewrite system* will be implement here. And the *node editor system* will be implement here too.

---

## About development

When you have prepared to program, please follow the [coding style](https://github.com/grass0916/DungeonGenerator/wiki/C%23-Style-Guide).

### Upload your code via Git

Keep the `master` branch is stable version. Any unstable work don't commit on `master`, checkout to another branch is better way. Merge it until your branch is finished without obvious bugs.

About the name of branch, is based on your task number. Example: You had finished the part of task **C1-2-3**, and name the branch **C1_2_3**.

We already set the [.gitignore](https://github.com/grass0916/DungeonGenerator/blob/master/.gitignore), you don't need remove the `*.meta` files each time.

```
# [Optional] If you want to create a new branch for your task.
> git branch <branch_name (Task number)>

# Switch to your branch.
> git checkout <branch_name>

# Add the files for this upload.
> git add <your_files>

# [Optional] Check the status of this action of adding.
> git status

# Add the comment in one line.
> git commit -m "Your comment for this upload."

# [First time] Commit to GitHub.
> git push -u https://github.com/grass0916/DungeonGenerator.git <branch_name>

# Commit to GitHub after first time.
> git push
```

### How to merge the branch to master?

NO. You don't need to do this. If you finish the task, please email to **Salmon** then he will confirm it. The link of the checklist [here](https://docs.google.com/spreadsheets/d/1A1DMKRhcZCAshqFJlascTw-KpFhdhQTCEFTGTjbah5s/edit?usp=sharing).
