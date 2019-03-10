# DotsAndBoxesBackend
The backend for the classic dots and boxes game. It consists of two projects:

1. DotsAndBoxes (ASP.NET Core 2.2)
2. DotsAndBoxes.Common (.Net Standard)

Some functionalities

   1. It supports any number of clients, when a connection is estalished to the hub the user can chose to create a new game providing the dimension, or join an active game if such exists
  
   2. Each time a move is made it passes validation, controls are frozen/enabled depending on the status (score, no score).
   
   3. If the winner is decided the game an alert is displayed and the page reloads.
   
   4. If an active player disconnects from the hub the appropriate message is sent to the other player and the game refreshes.
   
 Test Scenario:
 
   1. Run the project
   
   2. Open second browser and connect to the service
   
   3. Any of the two has to create a new game
   
   4. The other should have by now the join button enabled
   
   5. Game starts
   
   6. Each time the player has to add the coordinates and press play, while the other waits for his/her turn
 
 When the project runs an akward page appears. This and the game.js served as a dummy client to test the hub's functionality under no circumstance should these files remain as they are.
