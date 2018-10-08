const redis = require("redis"),
  client = redis.createClient();
const protobuf = require('protobufjs');

var readline = require('readline'),
  rl = readline.createInterface(process.stdin, process.stdout);

rl.setPrompt('OHAI> ');
rl.prompt();

rl.on('line', function (line) {
  protobuf.load("../protos/event.proto", function (err, root) {
    if (err)
      throw err;
    const events = root.lookupType("eventstore.MessageEvent");

    const payload = { name: "Hello from js", id: Math.floor(Math.random() * 100), firedDate: new Date().toLocaleString() };

    const errMsg = events.verify(payload);
    if (errMsg)
      throw Error(errMsg);
    const message = events.create(payload);
    const buffer = events.encode(message).finish();
    client.publish('messages', buffer);
  });
  rl.prompt();
});