package days

import (
	"fmt"
	"io/ioutil"
	"log"
	"time"
)

var (
	d23List []int
)

// Day23 runs day 23
func Day23() {
	start := time.Now()
	d23MakeList()
	log.Printf("Q23MakeList took %s\n", msDuration(start))
	start = time.Now()
	d23Part1()
	log.Printf("Q23Part1 took %s\n", msDuration(start))
	start = time.Now()
	d23Part2()
	log.Printf("Q23Part2 took %s\n", msDuration(start))
}

func d23MakeList() {
	bytes, err := ioutil.ReadFile("23input.txt")
	if err != nil {
		panic(err)
	}
	for _, ch := range string(bytes) {
		d23List = append(d23List, int(ch-'0'))
	}
}

func d23Part1() {

	cupList := make([]int, len(d23List)+1)
	currIdx := 0
	for i := 0; i < len(d23List); i++ {
		cupList[currIdx] = d23List[i]
		currIdx = d23List[i]
	}

	d23Solve(cupList, 100)

	cup := cupList[1]
	scoreStr := ""
	for cup != 1 {
		scoreStr += fmt.Sprintf("%d", cup)
		cup = getNextCup(cupList, cup)
	}

	log.Printf("Q23Part1: labels=%s\n", scoreStr)
}

func d23Part2() {
	cupList := make([]int, 1_000_001)
	currIdx := 0
	for _, cup := range d23List {
		cupList[currIdx] = cup
		currIdx = cup
	}
	highest := 0
	for idx := range d23List {
		if cupList[idx] == 0 {
			highest = idx
			break
		}
	}
	cupList[highest] = len(d23List) + 1
	for i := cupList[highest]; i < len(cupList); i++ {
		if i == len(cupList)-1 {
			cupList[i] = 0
		} else {
			cupList[i] = i + 1
		}
	}

	d23Solve(cupList, 10_000_000)

	first := cupList[1]
	second := cupList[first]
	log.Printf("Q23Part2: first=%d, second=%d, mult=%d", first, second, first*second)
}

func getNextCup(cupList []int, currCup int) int {
	next := cupList[currCup]
	if next == 0 {
		return getNextCup(cupList, next)
	}

	return next
}

func hasPickedUp(pickedUpCups []int, find int) bool {
	for _, val := range pickedUpCups {
		if val == find {
			return true
		}
	}

	return false
}

func d23Solve(cupList []int, iterations int) {
	currentCup := cupList[0]
	pickedUpCups := make([]int, 3)
	for move := 0; move < iterations; move++ {
		pickedUp := getNextCup(cupList, currentCup)
		nextCup := pickedUp
		for i := 0; i < len(pickedUpCups); i++ {
			pickedUpCups[i] = nextCup
			nextCup = getNextCup(cupList, nextCup)
		}

		cupList[currentCup] = nextCup

		destinationCup := currentCup - 1
		for hasPickedUp(pickedUpCups, destinationCup) || destinationCup == 0 {
			destinationCup--
			if destinationCup <= 0 {
				destinationCup = len(cupList) - 1
			}
		}

		oldDestinationNext := getNextCup(cupList, destinationCup)
		cupList[destinationCup] = pickedUp
		cupList[pickedUpCups[len(pickedUpCups)-1]] = oldDestinationNext
		currentCup = getNextCup(cupList, currentCup)
	}
}
