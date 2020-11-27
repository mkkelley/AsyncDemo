
N=200
TM=30
echo "Starting $N async requests to /wait/async/$TM"
date +%s

for (( i = 0; i < $N; i++ )); do

  curl -X GET http://localhost:5000/wait/async/$TM &

done

wait

echo
date +%s
echo "waiting 25 seconds for reset"
sleep 25

echo "Starting $N sync requests to /wait/sync/$TM"
date +%s

for (( i = 0; i < $N; i++ )); do

  curl -X GET http://localhost:5000/wait/sync/$TM &

done

wait

echo
date +%s
echo "waiting 25 seconds for reset"
sleep 25

echo "Starting $N blocking async requests to /wait/async-block/$TM"
date +%s

for (( i = 0; i < $N; i++ )); do

  curl -X GET http://localhost:5000/wait/sync/$TM &

done

wait

echo
date +%s
