//Modules
var io = require('socket.io')(process.env.PORT || 52300);
var Player = require('./Classes/Player');

console.log('Server has started');

var playersList = [];
var sockets = [];
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

    //Update position from client
    socket.on('updatePosition', function(data){
        console.log(data.id);
        thisPlayer.position.x = data.position.x;
        thisPlayer.position.y = data.position.y;
        thisPlayer.position.z = data.position.z;
        socket.broadcast.emit('updatePosition', thisPlayer);
    });
    //Tell this player about other players
    for(var playerID in playersList){
        if(playerID != thisPlayerID){
            socket.emit('spawn', playersList[playerID]);
        }
    }
    socket.on('disconnect', function(){
        console.log('A player has disconnected');
        delete playersList[thisPlayerID];
        delete sockets[thisPlayerID];
        socket.broadcast.emit('disconnected', thisPlayer);
    });
});