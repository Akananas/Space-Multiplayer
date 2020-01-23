//Modules
var io = require('socket.io')(process.env.PORT || 52300);
var Player = require('./Classes/Player');
var Bullet = require('./Classes/Bullet');

console.log('Server has started');

var playersList = [];
var sockets = [];
var bullets = [];

//Updates
setInterval(() => {
    bullets.forEach(bullet => {
        var isDestroyed = bullet.onUpdate();
        //Remove
        if(isDestroyed){
            var index = bullets.indexOf(bullet);
            if(index > -1){
                bullets.splice(index,1);
                var returnData = {
                    id: bullet.id
                }
                for(var playerID in playersList){ 
                    sockets[playerID].emit('serverUnspawn', returnData);
                }
            }
        }else{
            var returnData = {
                id: bullet.id,
                position: {
                    x: bullet.position.x, 
                    y: bullet.position.y,
                    z: bullet.position.z  
                }
            }
            for(var playerID in playersList){
                sockets[playerID].emit('updateBullet', returnData);
            }
        }
    });
}, 100, 0);
io.on('connection', function(socket){
    console.log('Connection made!'); 

    var thisPlayer = new Player();
    var thisPlayerID = thisPlayer.id;
    playersList[thisPlayerID] = thisPlayer;
    sockets[thisPlayerID] = socket;

    //Tell the client that this is our id for the server
    socket.emit('register', {id: thisPlayerID});
    socket.emit('spawn', thisPlayer);//Tell this player he spawned
    socket.broadcast.emit('spawn', thisPlayer);//Tell other this player spawned
    
    //Tell this player about other players
    for(var playerID in playersList){
        if(playerID != thisPlayerID){
            socket.emit('spawn', playersList[playerID]);
        }
    }
    //Update position from client
    socket.on('updatePosition', function(data){
        thisPlayer.position.x = data.position.x;
        thisPlayer.position.y = data.position.y;
        thisPlayer.position.z = data.position.z;
        thisPlayer.rotation = data.rotation;
        socket.broadcast.emit('updatePosition', thisPlayer);
    });
    socket.on('fireBullet', function(data){
        var bullet = new Bullet();
        bullet.name = 'Bullet';
        bullet.activator = data.activator;
        bullet.position.x = data.position.x;
        bullet.position.y = data.position.y;
        bullet.position.z = data.position.z;
        bullet.direction.x = data.direction.x;
        bullet.direction.y = data.direction.y;
        bullet.direction.z = data.direction.z;
        console.log(bullet.direction.x);
        console.log(bullet.direction.y);
        console.log(bullet.direction.z);
        bullets.push(bullet);

        var returnData = {
            name: bullet.name,
            id: bullet.id,
            activator: bullet.activator,
            position: {
                x: bullet.position.x,
                y: bullet.position.y,
                z: bullet.position.z
            },
            direction: {
                x: bullet.direction.x,
                y: bullet.direction.y,
                z: bullet.direction.z
            }
        }
        socket.emit('serverSpawn', returnData);
        socket.broadcast.emit('serverSpawn', returnData);
    });
    socket.on('collisionDestroy', function(data){
        console.log('Collision with bullet id: ' + data.id);
        let returnBullets = bullets.filter(bullet => {
            return bullet.id == data.id;
        });
        returnBullets.forEach(bullet =>{
            bullet.isDestroyed = true;
        });
    });
    socket.on('disconnect', function(){
        console.log('A player has disconnected');
        delete playersList[thisPlayerID];
        delete sockets[thisPlayerID];
        socket.broadcast.emit('disconnected', thisPlayer);
    });
});

function interval(func, wait, time){
    var interv = function(w, t){
        return function(){
            if(typeof t === "undefined" || t-- > 0){
                setTimeout(interv, w);
                try{
                    func.call(null);
                }catch(e){
                    t = 0;
                    throw e.toString();
                }
            }
        };
    }(wait,times);

    setTimeout(interv,wait);
}