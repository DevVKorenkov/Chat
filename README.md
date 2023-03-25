Hi!
This is the test task project for "Reaction games" game studio.

This is the Chat with clans. It's base is asp.net with signalR and Angular.
It has a registration and loggining. To enter, you have to sign up. 
There are two chats which users can use. Common chat (doesn't require a clan membership) and the clan chat (surprisingly, it requires to be in a clan).
Also, you can see the clans list on the home page. You can click each of them and see the list of members.
And, you can join the clan just pushing the "Apply" button.

One user can only be in one chat. If you signin with another browser and try to enter some chat, you will see an alert.

This project uses MsSql as a general data store and redis as a quick cache store for messages.
The back-end is .net 7 and front-end is Angular 15 (I am still a bad front-end-developer, so don't pay much attention on it. 
And, actually, I haven't resolved a couple problems with front-end. And here they are: 
1. somehow, the redirection to home page doesn't work after authorization or registration. You should write in the url /home instead of /auth by your own hands or just delete /auth from url and hit enter after one of these actions.
2. Messages come instantly (you can see it in browser developer console), but the view is slow. Sometimes it can take more than 15 seconds.)

To execute it you should have MsSQL data base and redis which must be specified in appsettings.json or appsettings.Development.json if you are in developer mode.

For my shame I didn't make fault tolerance ability because I don't know how. Sorry for that.
Also I don't like my messages cache solution, but it should work fast, I guess.

Please, have fun with it, and I'll try to accept any criticism :)
Thank you!
