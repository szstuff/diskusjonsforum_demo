A modern and simple discussion forum MVC webapp powered by .NET 6.0, EntityFrameworkCore*, and AspNetCore Identity*.
This is a modified version developed for the live demo available on stilian.dev. 

*This version uses a session cookie instead of ASP Identity for user management. It also does not display content generated by users unless they have the same cookie. However, there is no access control and the webapp is vulnerable to HTTP Parameter Pollution attacks. This is not a concern as this version is only used for demo purposes.
*This version has a simplified version of the database and class models. 

See the fully-featured version here: https://github.com/szstuff/diskusjonsforum_ITPE3200

## Basic Functionality

The application supports three usertypes: Viewers, Users, and Admins, with the following capabilities:

### Viewers (Unregistered / Not Logged In)
- **View Threads and Comments**.
- **Filter Threads by Category**.
- **Search for Threads**: Find threads by keywords or phrases.
- **Register**.

### Users (Registered and Logged In)
- **Login/Logout**.
- **Create, View, Edit, and Delete Discussion Threads**.
- **Create, View, Edit, and Delete Discussion Comments**.
- **Toggle Admin Role** (Testing Purposes): Switch between User and Admin roles.
- And functionality available to Viewers

### Admins
- **Access User Table**: View a table displaying all registered users via `/User/Table`.
- **Delete and edit Threads**: Edit other user's threads from the forum for moderation purposes.
