var shortid = require('shortid');
var Vector3 = require('./Vector3');
module.exports = class Player{
    constructor(){
        this.username = '';
        this.id = shortid.generate();
        this.position = new Vector3();
        this.rotation = 0;
        this.health = new Number(100);
        this.isDead = false;
        this.respawnTicker = new Number(0);
        this.respawnTime = new Number(0);
    }
    respawnCounter(){
        this.respawnTicker++;
        if(this.respawnTicker >= 10){
            this.respawnTicker = new Number(0);
            this.respawnTime++;

            //Three second respawn
            if(this.respawnTime >= 3){
                console.log('Respawning player with id: '+ this.id);
                this.isDead = false;
                this.respawnTicker = new Number(0);
                this.respawnTime = new Number(0);
                this.health = new Number(100);
                this.position = new Vector3(0,0,0);
                return true;
            }
        } 
        return false;
    }
    takeDamage(amount = Number){
        console.log('Taking damage');
        //Change health when getting hit
        this.health -= amount;
        console.log(this.health); 
        //Check if it kills
        if(this.health <= 0){
            this.isDead = true;
            this.respawnTicker = new Number(0);
            this.respawnTime = new Number(0);
        }
        return this.isDead;
    }
}