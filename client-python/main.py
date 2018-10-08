import redis
import event_pb2

r = redis.StrictRedis(host='localhost', port=6379, db=0)

while True:
    value = input()
    event = event_pb2.MessageEvent()
    event.id = 1
    event.name = value + " python"

    if value == "q":
        break

    data = event.SerializeToString()
    r.publish('messages', data)