using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ProceduralGenerationAlgorithms {

    
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength) {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPosition);
        var previousPosition = startPosition;

        for(int x = 0; x < walkLength; x++) {
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }
        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength) {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.GetRandomCardinalDirection();
        var currentPosition = startPosition;
        corridor.Add(currentPosition);

        for (int x = 0; x < corridorLength; x++) {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }
        return corridor;
    }

    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight, int bossRoomWidth, int bossRoomHeight, int number_of_rooms) {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit);

        // while(roomsQueue.Count > 0) = generates a random room count
        // while (roomsList.Count < number_of_rooms) = generates rooms until there is the proper amount
        int count = 0;
        while(roomsList.Count < number_of_rooms) {
            // Debug.Log("Count " + count);
            if (count > 10000) {
                Debug.Log("BIG COUNT 10,000");
                break;
            } else {
                var room = roomsQueue.Dequeue();

                // Debug.Log("Room width = " + room.size.x);
                // Debug.Log("Room height = " + room.size.y);

                // Debug.Log("Min Width = " + minWidth);
                // Debug.Log("Room Size = " + room.size.y);
                // if (roomsList.Count != number_of_rooms - 1) {
                    if (room.size.y >= minHeight && room.size.x >= minWidth) {
                        if (Random.value < 0.5f) {
                            if (room.size.y >= minHeight * 2) {
                                SplitHorizontally(minHeight, roomsQueue, room);
                            } else if (room.size.x >= minWidth * 2) {
                                SplitVertically(minWidth, roomsQueue, room);
                            } else {
                                // Debug.Log("Room width = " + room.size.x);
                                // Debug.Log("Room height = " + room.size.y);
                                roomsList.Add(room);
                            }
                        } else {
                            if (room.size.x >= minWidth * 2) {
                                SplitVertically(minWidth, roomsQueue, room);
                            } else if (room.size.y >= minHeight *2) {
                                SplitHorizontally(minHeight, roomsQueue, room);
                            } else if (room.size.x >= minWidth && room.size.y >= minHeight) {
                                // Debug.Log("Room width = " + room.size.x);
                                // Debug.Log("Room height = " + room.size.y);
                                roomsList.Add(room);
                            }
                        }
                    }
                // } else {
                //     if (room.size.y >= bossRoomHeight && room.size.x >= bossRoomWidth) {
                //         if (Random.value < 0.5f) {
                //             if (room.size.y >= bossRoomHeight * 2) {
                //                 SplitHorizontally(bossRoomHeight, roomsQueue, room);
                //             } else if (room.size.x >= bossRoomWidth * 2) {
                //                 SplitVertically(bossRoomWidth, roomsQueue, room);
                //             } else {
                //                 // Debug.Log("Room width = " + room.size.x);
                //                 // Debug.Log("Room height = " + room.size.y);
                //                 roomsList.Add(room);
                //             }
                //         } else {
                //             if (room.size.x >= bossRoomWidth * 2) {
                //                 SplitVertically(bossRoomWidth, roomsQueue, room);
                //             } else if (room.size.y >= bossRoomHeight *2) {
                //                 SplitHorizontally(bossRoomHeight, roomsQueue, room);
                //             } else if (room.size.x >= bossRoomWidth && room.size.y >= bossRoomHeight) {
                //                 // Debug.Log("Room width = " + room.size.x);
                //                 // Debug.Log("Room height = " + room.size.y);
                //                 roomsList.Add(room);
                //             }
                //         }
                //     }
                // }
            count++;
            }
        }
        return roomsList;
    }

    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room) {    
        var xSplit = Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
            new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room) {
        var ySplit = Random.Range(1, room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z), 
            new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}

public static class Direction2D {
    public static List<Vector2Int> cardinalDirectionList = new List<Vector2Int> {
        new Vector2Int(0, 1), // Up
        new Vector2Int(1, 0), // Right
        new Vector2Int(0, -1), // Down
        new Vector2Int(-1, 0) // Left
    };

    public static List<Vector2Int> diagonalDirectionList = new List<Vector2Int> {
        new Vector2Int(1, 1), // Up-Right
        new Vector2Int(1, -1), // Right-Down
        new Vector2Int(-1, -1), // Down-Left
        new Vector2Int(-1, 1) // Left-Up
    };

    public static List<Vector2Int> eightDirectionsList = new List<Vector2Int> {
        new Vector2Int(0, 1), // Up
        new Vector2Int(1, 1), // Up-Right
        new Vector2Int(1, 0), // Right
        new Vector2Int(1, -1), // Right-Down
        new Vector2Int(0, -1), // Down
        new Vector2Int(-1, -1), // Down-Left
        new Vector2Int(-1, 0), // Left
        new Vector2Int(-1, 1) // Left-Up
    };

    public static Vector2Int GetRandomCardinalDirection() {
        return cardinalDirectionList[Random.Range(0, cardinalDirectionList.Count)];
    }
}
