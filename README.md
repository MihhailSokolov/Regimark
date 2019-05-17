# Regimark
C# Desktop application for a school teacher to ease assessment of oral answers during the lesson

## How to run
- Either open Regimark.sln in your Visual Studio and press `Start` button
- Or go to Regimark/obj/Release and run `Regimark.exe`

## How to use
- When you run the app, you need to upload to it a file with the list of the names of your students in the same format as they are sitting
- Then either specify a file name for the new file with the marks you will put or choose an old file which might already contain some marks
- Then the class screen is loaded with every student having 2 buttons next to him/her `+` and `-`
- When the student answers you either press `+` for correct answer or `-` for incorrect one
- When certain student answers 3 questions, he will get a mark based on his/her answers:
  - 3 correct answers is 5
  - 2 correct answers and 1 incorrect is 4
  - 2 incorrect answers and 1 correct is 3
  - 3 incorrect answers is 2
- All the marks are immideately written to the file and everything is saved for later use
