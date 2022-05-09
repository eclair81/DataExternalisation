
# DataExternalisation

A class assignement.

The objective is to externalise the data of an application to allow an end-user to 
swap them with others without needing to recompile the project.

My project is a platformer. Each level is represented by a png and each pixel 
(except for the transparent ones) is corresponding to an element.

## Data
- Player.json
    - life (base number of lives)
    - CoinForLife (number of coins needed to gain an extra life)
    - acceleration (the higher, the quicker the player'll reach it's maxSpeed)
    - maxSpeed (max horizontal speed of the player)
    - deceleration (the higher, the quicker the player'll come to a complete stop)
    - jumpHeight (the higher, the higher the player'll jump, exponential)
    - coyoteTime (delay, in seconds, where you can still jump, as if you were still on the ground, after leaving the ground)
    - src (path to the player's spritesheet. You can either specify 5 sheets (one for each animation: Idle, Run, Jump, Fall and Death, in that order) or only 1. If you give 5 sheets, delete the "info" entry from the json)
    - dim ( [width, height] of the sprites in the sheet(s) specified in "src". All the sprites must have the same format)
    - info (add this entry if all your animations are on the same sheet. follows this template: [x, y, n, x2, y2, n2, ...] where x and y are the coordinates, in the sheet, of the first sprite of your animation and n the number of sprites in that animation. Top-left corner sprite is x:0, y:0)
    - animDelay (delay, in seconds, before switching to the next sprite when playing an animation) 
    - extraLifeSound (path to the audio to play when gaining an extra life)
    - deathSound (path to the audio to play when the player falls off the stage)
    - volume ( [0f-1f, 0f-1f] volume for "extraLifeSound" and "deathSound". 0 is mute and 1 is max volume. if you delete this entry, base volume is 1)
    
- Stages.json
    - stageList (string array with the path of each level)
    - colorFidelity (threshold, used to compare the colors read by unity and the colors specified in "colorMapping". keep low)
    - colorMapping (array of elements)
        - nom (name of the element)
        - rvb ( [r, g, b] color values of that element)
        - src (path to the sheet associated to that element. "bloc" type takes 3 sheets of 3 sprites each, "checkpoint" and "collectible" takes 2 sheets and "endPoint" takes 1 sheet. "spawnPoint" doesn't use one)
        - info (same as for Player.json)
        - dim (same as for Player.json)
        - scale ( [scaleX, scaleY], only for elements of type "checkPoint" / "endPoint" / "collectible". don't go above 1 for either scaleX or scaleY)
        - type ("spawnPoint" / "checkPoint" / "endPoint" / "bloc" / "collectible")
        - animDelay (same as for Player.json . Not for "bloc" / "spawnPoint" type)
        - sound (path to the audio to play when interacting with that element. Not for "bloc" / "spawnPoint" / "endPoint"type)
        - volume ( [0f-1f] volume for the "sound" entry. 0 is mute and 1 is max volume. if you delete this entry, base volume is 1)

## Add a new stage, change the look
Add new PNGs inside the StreamingAssets folder and update Stages.json accordingly to add your custom stages.
Add your custom sheets inside the StreamingAssets folder and update Player/Stages.json accordingly to change the look of the game.
Create new elements inside of colorMapping to add more variety to your stages.

## Acknowledgements

 - [Tarodev's Ultimate 2D Controller](https://github.com/Matthew-J-Spencer/Ultimate-2D-Controller)

## Warning
The sound isn't working with StreamingAssets yet. Don't touch/modify sound path related entries