module.exports = class Vector3{
    constructor(X=0,Y=0,Z=0){
        this.x = X;
        this.y = Y;
        this.z = Z;
    }

    Magnitude(){
        return Math.sqrt((this.x * this.x) + (this.y * this.y) + (this.z * this.z));
    }

    Normalized(){
        var mag = this.Magnitude();
        return new Vector3(this.x/mag, this.y/mag, this.z/mag);
    }

    Distance(otherVector = Vector3){
        var direction = new Vector3();
        direction.x = otherVector.x - this.x;
        direction.y = otherVector.y - this.y;
        direction.z = otherVector.z - this.z;
        return direction.Magnitude();
    }

    ConsoleOutput(){
        return '('+this.x+','+this.y+','+this.z+')';
    }
}