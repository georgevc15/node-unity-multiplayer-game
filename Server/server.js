var io = require('socket.io')(process.env.PORT || 3000);
var shortid = require('shortid');

console.log('Server started');

var players = [];

var playerSpeed = 3;

io.on('connection', function(socket) {
	
	var thisPlayerId = shortid.generate();

	var player = {
		 id: thisPlayerId,
			 destination: {
				 x: 0, 
				 y: 0
			},
			lastPosition: {
				 x: 0, 
				 y: 0
			}
	};

	players[thisPlayerId] = player;

	console.log('Client connected, broadcasting spawn id', thisPlayerId);

	socket.emit('register',  { id: thisPlayerId })
	socket.broadcast.emit('spawn', { id: thisPlayerId });
	socket.broadcast.emit('requestPosition');


	for(var playerId in players) {

		if(playerId == thisPlayerId) 
			continue;

		socket.emit('spawn', players[playerId]);
		console.log('sending spawn to new player for id:', playerId);
	};


	socket.on('move', function(data) {
		data.id = thisPlayerId;
		console.log('Client moved', JSON.stringify(data));

		player.destination.x = data.d.x;
		player.destination.y = data.d.y;

			console.log("Distance between current and destination", lineDistance(data.c, data.d));

		delete data.c;

		data.x = data.d.x;
		data.y = data.d.y;

		delete data.d;


		socket.broadcast.emit('move', data);	
	});

		socket.on('follow', function(data) {
			console.log("follow request: ", data);
			data.id = thisPlayerId;	
			
			socket.broadcast.emit('follow', data);
		});

		socket.on('updatePosition', function(data) {
			console.log("update position", data);
			data.id = thisPlayerId;	
			
			socket.broadcast.emit('updatePosition', data);
		});

		socket.on('attack', function(data) {
			console.log("attack request", data);
			data.id = thisPlayerId;	
			
			io.emit('attack', data);
		});

		socket.on('disconnect', function() {
			console.log('Client diconnected');

			//players.splice(players.indexOf(thisPlayerId), 1);
			delete players[thisPlayerId];

			socket.broadcast.emit('disconnected', {id: thisPlayerId});
	})
});

function lineDistance(vectorA, vectorB) {
	var xs = 0;
	var ys = 0;

	xs = vectorB.x - vectorA.x;
	xs = xs * xs;

	ys = vectorB.y - vectorA.y;
	ys = ys * ys;

	return Math.sqrt( xs + ys );
}