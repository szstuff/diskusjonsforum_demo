A modern and simple discussion forum MVC webapp powered by .NET 6.0, EntityFrameworkCore, and AspNetCore Identity.

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

## Video Demonstrations

1. **Access control**
2. **Viewing and Filtering Threads**
3. **Searching for Threads**
4. **Creating and Editing Threads**
5. **Commenting on Threads**

