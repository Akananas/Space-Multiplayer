var ServerObject = require('./ServerObject');
var Vector3 = require('./Vector3');
var Utility =  require('./Utility');

module.exports = class Bullet extends ServerObject{
    constructor(){
        super();
        this.direction = new Vector3(); 
        this.speed = 7.5;
        this.isDestroyed = false;
        this.activator = '';
    }

    onUpdate(){ 
        //Trouble with comma and period
        this.position.x = Utility.Period(Utility.Comma(this.position.x) + 
                           Utility.Comma(this.direction.x) * this.speed);
        this.position.z = Utility.Period(Utility.Comma(this.position.z) + 
                           Utility.Comma(this.direction.z) * this.speed); 
        return this.isDestroyed;
    }
}