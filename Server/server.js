//Modules
var io = require('socket.io')(process.env.PORT || 52300);
var Player = require('./Classes/Player');
var Bullet = require('./Classes/Bullet');
var Utility = require('./Classes/Utility');

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
            despawnBullet(bullet);
        }
    });
    //Handle dead players
    for(var playerID in playersList){
        let player = playersList[playerID];
        if(player.isDead){
            let isRespawn = player.respawnCounter();
            if(isRespawn){
                let returnData = {
                    id: player.id
                };
                sockets[playerID].emit('playerRespawn',returnData);
                sockets[playerID].broadcast.emit('playerRespawn',returnData);
            }
        }
    }
}, 100, 0);

function despawnBullet(bullet = Bullet){
    console.log('Destroying bullet with id: ' + bullet.id);
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
}
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
        var returnData = {
            id: thisPlayer.id,
            rotation: thisPlayer.rotation,
            position: {
                x: thisPlayer.position.x,
                y: thisPlayer.position.y,
                z: thisPlayer.position.z
            }
        };
        socket.broadcast.emit('updatePosition', returnData);
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
            },
            speed: Utility.Period(bullet.speed)
        };
        socket.emit('serverSpawn', returnData);
        socket.broadcast.emit('serverSpawn', returnData);
    });
    socket.on('collisionDestroy', function(data){
        console.log('Collision with bullet id: ' + data.id);
        let returnBullets = bullets.filter(bullet => {
            return bullet.id == data.id;
        });
        returnBullets.forEach(bullet =>{
            let playerHit = false;
            //Check we don't hit ourself
            for(var playerID in playersList){
                if(bullet.activator != playerID){
                    let player = playersList[playerID];
                    let distance = bullet.position.Distance(player.position);
                    if(distance < 0.2){  
                        playerHit = true; 
                        let isDead = player.takeDamage(50);
                        if(isDead){
                            console.log('Player with id: ' + player.id + ' has died');
                            let returnData = {
                                id: player.id
                            };
                            sockets[playerID].emit('playerDied', returnData);
                            sockets[playerID].broadcast.emit('playerDied', returnData);
                        }else{
                            console.log('Player with id: ' + player.id + ' has ' + player.health + ' health left'); 
                        }
                        despawnBullet(bullet);
                    }
                } 
            }
            if(!playerHit){
                bullet.isDestroyed = true;
            }
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