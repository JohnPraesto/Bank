BANK

Introduction

This project is a bank simulator in which the user first needs to become a customer by providing a username and a password. The username and password is stored in an array, which in turn is stored in a list of arrays. When the user logs in, he or she is presented with a menu of normal bank services, like creating new accounts, and depositing, withdrawing, and transfering money. The user accounts are stored in an Account class object, consisting of string username (to determine who is the owner of the account) and string account name (chosen by the user) and double sum (storing the account balance).



Reflection

There are two main reasons that determinied how I approached this project – my ambition level and the time I had to work it. First off, I’m taking this course to learn, not to get a pass. That’s why I strive to make my projects include all functions asked for in the project description, and more, preferably the extra fucntions too. And maybe, if I’m caught by an interesting idea, I will try to implement that too. However, I had a vacation last week. And when I returned, I had only a couple of days to work on this project. To me, that’s not enough. I want to try ideas, try functions, read some articles, discuss with student collegaues (STAVNING) and discuss with chatGPT, to eventually form the most effective and compact code solving the task. Therefore I usually start my project well in advance. This was unfortunately (STAVNING) not possible for me this time. In this project I hasted through solutions. I just went along with the first code that came to mind, even if I didn’t like it, as long as it made the program work as it was supposed to. I made a rackity (STAVNING) wooden plank house, full of nails and duct tape.

Towards the end of the Bank project, when many of the project fucntions were included and functioning, it was only then I took the time to look at the code with critical eyes, with demanding eyes, and judging eyes. I had almost 700 lines of code. I was able to work it down to 400, mostly by creating methods for code reusability, but also just re-writing many lines, changing variable names, re-working loops etc.

What I am most sorry to have omitted adding to the Bank is information saving. I would have liked that the users created, their accounts and their balance did not reset when the program closed. I was not quite sure how it would be done. But in a previous course, Programmering 2, we worked briefly with StreamReader and StreamWriter. Creating textdocuments in the project folder in which all this information could be stored. I chose not to try to implement this in the project due to shortage of time.

What was hardest making work in this project, and finding a solution to, was how the users, their passwords, and their accounts would be created, and how they would be connected to eachother. I solved this by first making an array, consisting of two elements, both strings, one for the user name, and one for the password. This new array would be stored in a list of arrays. That means that the index number of the array in the list would be the doorway to both elements in that array, namely the username and that user’s password. As explained in the introduction, the account objets include the username. And that’s where the connection between the account and user lies. For example, iterating through the list of users and the list of accounts, looking for a certain username, you find both the user and their accounts. However, even if I don’t have a clear suggestion on how it would be made, I have a feeling of that the code could be more streamlined or effective, if all information of a user was collected in the same object. Maybe, somehow, the password and the users list of accounts could be included in the same object?

And I am not pleased with the ”depth” of the code. There are too many ifs in whiles in foreaches in switches etc. I don’t really know why I dont like it. Maybe it makes the code harder to navigate through, comparing to another way of constructing the code, whatever that would look like. Maybe it’s because it’s just a first hand, not thought through, solution. It’s like just thinking one step at a time. Or maybe it’s because if something goes wrong within the tree of ifs, much of the tree would have to be re-made. I’m looking forward to know what is the common practice and opinion in this matter.
	
It has been hard to work with this multide of code lines. Maybe more methods, and more classes, not mentioning just smarter solutions (of which I haven’t though of), would make this project easier to work with, in both overview and binding together parts. Projects need to be thought through and planned.

John Praesto SUT-23
