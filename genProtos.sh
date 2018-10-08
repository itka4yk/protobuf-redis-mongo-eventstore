protoc --python_out=client-python/ protos/event.proto
protoc --csharp_out=client-csharp/ protos/event.proto
protoc --csharp_out=server-csharp/ protos/event.proto