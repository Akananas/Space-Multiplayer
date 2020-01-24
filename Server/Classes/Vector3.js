var Utility = require('./Utility');
module.exports = class Vector3{
    constructor(X,Y,Z){
        this.x = X;
        this.y = Y;
        this.z = Z;
    }

    Magnitude(){ 
        return Math.sqrt(Utility.Comma(this.x) * Utility.Comma(this.x) +
                         Utility.Comma(this.y) * Utility.Comma(this.y) +
                         Utility.Comma(this.z) * Utility.Comma(this.z));
    }

    Normalized(){
        var mag = this.Magnitude();
        var tmpVec = new Vector3;
        return tmpVec.FloatVectorNorm(this.x,this.y,this.z,this.mag);
    }

    Distance(otherVector = Vector3){
        var direction = new Vector3();
        direction.FloatVector(Utility.Comma(otherVector.x) - Utility.Comma(this.x),
                              Utility.Comma(otherVector.y) - Utility.Comma(this.y),
                              Utility.Comma(otherVector.z) - Utility.Comma(this.z));
        return direction.Magnitude()/100; 
    }

    ConsoleOutput(){
        return '('+this.x+','+this.y+','+this.z+')';
    }
 
    FloatVectorNorm(X,Y,Z,mag){
        this.x = Utility.Period(X/mag);
        this.y = Utility.Period(Y/mag);
        this.z = Utility.Period(Z/mag);
    }
    FloatVector(X,Y,Z){
        this.x = Utility.Period(X);
        this.y = Utility.Period(Y);
        this.z = Utility.Period(Z);
    }
    
}