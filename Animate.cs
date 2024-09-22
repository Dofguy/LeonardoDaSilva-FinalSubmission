using UnityEngine;

public class Animate : MonoBehaviour
{
    public SpriteRenderer spriteRend;
    public bool loop = true; //looping sprites e.g. Pacwoman
    public Sprite[] sprites; //Holds all sprites of object to loop through
    public float animateTime = 0.25f; //Animation time between sprites
    public int animateFrame; //Tracks frame we're currently on

    private void Start(){
        this.spriteRend = GetComponent<SpriteRenderer>(); //Makes sure the object has a sprite renderer to work
        InvokeRepeating(nameof(Next), this.animateTime, this.animateTime); //Repeatedly calls the next function every animated time to move through frames
    }

    private void Next(){
        // Increment the frame
        this.animateFrame++;

        // If looping is enabled, use modulo to handle frame overflow and resetting
        if (this.loop){
            this.animateFrame %= this.sprites.Length;
        }

        // Set the sprite renderer's sprite to the current frame of the animation, if within the valid range
        if (this.animateFrame >= 0 && this.animateFrame < this.sprites.Length){
            this.spriteRend.sprite = this.sprites[this.animateFrame];
        }
    }
}