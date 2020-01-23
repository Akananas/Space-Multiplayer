var ServerObject = require('./ServerObject');
var Vector3 = require('./Vector3');

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
        this.position.x = (this.Comma(this.position.x) + 
                           this.Comma(this.direction.x) * this.speed).toString().replace(".",",");
        this.position.z = (this.Comma(this.position.z) + 
                           this.Comma(this.direction.z) * this.speed).toString().replace(".",","); 
        return this.isDestroyed;
    }

    Comma(data){
        return parseFloat(data.replace(",",".")); 
    }
}