using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteSheetCreator : object
{
    //public static int width = 32;
    //public static int height = 32;

    // return an array of (width * height) sprites 
    // all the sprites in fullSheet have to be in line
    public static Sprite[] CreateSpriteSheet(Texture2D fullSheet, int width, int height)
    {
        int nbSprites = (int)(fullSheet.width / width);
        Sprite[] spriteSheet = new Sprite[nbSprites];
    
        for (int x = 0; x < nbSprites; x++)
        {
            Sprite newSprite = Sprite.Create(
                fullSheet,
                new Rect(x * width, 1 * height, width, height),
                new Vector2(width / 2, height / 2) // -> pivot point (center)
            );
 
            spriteSheet[x] = newSprite;
        }
        return spriteSheet;
    }

    // return an array of (width * height) sprites
    // cooX / cooY -> coordinates of the first sprite to extract in the sheet (ex: 2nd sprite on the 3rd row -> cooX = 2 and cooY = 3)
    // number -> number of Sprites to extract (must be positive)
    // /!\ fullSheet should not contain empty spots where sprites are being extracted
    public static Sprite[] ExtractSpriteSheet(Texture2D fullSheet, int width, int height, int cooX, int cooY, int number)
    {
        Sprite[] spriteSheet = new Sprite[number];
        int currentNumberExtracted = 0;
        int x = cooX;
        int y = cooY;

        while (currentNumberExtracted != number)
        {
            Sprite newSprite = Sprite.Create(
                fullSheet,
                new Rect(x * width, y * height, width, height),
                new Vector2(width / 2, height / 2) // -> pivot point (center)
            );

            spriteSheet[currentNumberExtracted] = newSprite;

            //positionning x and y to extract next sprite
            
            x = (x + 1) % ((int)(fullSheet.width / width));
            if(x == 0) // changing row
            {
                y++;
            }

            currentNumberExtracted++;
        }
        return spriteSheet;
    }
}
