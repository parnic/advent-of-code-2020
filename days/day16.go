package days

import (
	"fmt"
	"io/ioutil"
	"log"
	"regexp"
	"strconv"
	"strings"
	"time"
)

const ()

var (
	rules        = make(map[string][]int)
	myTicket     = make([]int, 0)
	otherTickets = make([][]int, 0)
	ruleRegex    = regexp.MustCompile(`(.+): (\d+)-(\d+) or (\d+)-(\d+)`)
)

func atoi(val string) int {
	iVal, err := strconv.Atoi(val)
	if err != nil {
		panic(err)
	}

	return iVal
}

func atol(val string) int64 {
	iVal, err := strconv.ParseInt(val, 10, 64)
	if err != nil {
		panic(err)
	}

	return iVal
}

func msDuration(startTime time.Time) float64 {
	return float64(time.Since(startTime)) / 1000000
}

// Day16 runs day 16
func Day16() {
	start := time.Now()
	makeList()
	log.Printf("Q16MakeList took %fms\n", msDuration(start))
	start = time.Now()
	part1()
	log.Printf("Q16Part1 took %fms\n", msDuration(start))
	start = time.Now()
	part2()
	log.Printf("Q16Part2 took %fms\n", msDuration(start))
}

func makeList() {
	bytes, err := ioutil.ReadFile("16input.txt")
	if err != nil {
		panic(err)
	}
	fileStr := string(bytes)
	mode := 0
	for _, line := range strings.Split(fileStr, "\n") {
		line = strings.TrimSpace(line)
		if len(line) == 0 {
			mode++
			continue
		}

		switch mode {
		case 0:
			matches := ruleRegex.FindStringSubmatch(line)
			if matches == nil {
				panic("no regex match")
			}

			rules[matches[1]] = []int{
				atoi(matches[2]),
				atoi(matches[3]),
				atoi(matches[4]),
				atoi(matches[5]),
			}
			break

		case 1:
			if line == "your ticket:" {
				continue
			}

			for _, val := range strings.Split(line, ",") {
				myTicket = append(myTicket, atoi(val))
			}
			break

		case 2:
			if line == "nearby tickets:" {
				continue
			}

			otherTicket := make([]int, 0)
			for _, val := range strings.Split(line, ",") {
				otherTicket = append(otherTicket, atoi(val))
			}

			otherTickets = append(otherTickets, otherTicket)
			break

		case 3:
			panic("unexpected")
		}
	}
}

func isValidForAnyRule(val int) bool {
	for rule := range rules {
		if isValidForRule(val, rule) {
			return true
		}
	}

	return false
}

func isValidForRule(val int, rule string) bool {
	if (val >= rules[rule][0] && val <= rules[rule][1]) || (val >= rules[rule][2] && val <= rules[rule][3]) {
		return true
	}

	return false
}

func isValidTicket(ticket []int) bool {
	for _, val := range ticket {
		if !isValidForAnyRule(val) {
			return false
		}
	}

	return true
}

func part1() {
	invalidValues := make([]int, 0)
	for _, ticket := range otherTickets {
		for _, val := range ticket {
			if !isValidForAnyRule(val) {
				invalidValues = append(invalidValues, val)
			}
		}
	}

	result := 0
	for _, val := range invalidValues {
		result += val
	}

	log.Printf("Q16part1: invalid sum=%d\n", result)
}

func part2() {
	validTickets := make([][]int, 0)
	for _, ticket := range otherTickets {
		if isValidTicket(ticket) {
			validTickets = append(validTickets, ticket)
		}
	}

	ruleMatches := make(map[string][]int)

	for ruleName := range rules {
		numCols := len(validTickets[0])
		for checkCol := 0; checkCol < numCols; checkCol++ {
			ruleValid := true
			for _, ticket := range validTickets {
				if !isValidForRule(ticket[checkCol], ruleName) {
					ruleValid = false
					break
				}
			}

			if ruleValid {
				ruleMatches[ruleName] = append(ruleMatches[ruleName], checkCol)
			}
		}

		if _, ok := ruleMatches[ruleName]; !ok {
			panic("didn't map column to rule")
		}
	}

	finalMap := make(map[string]int)
	ruleMatched := func(test string) bool {
		for name := range finalMap {
			if name == test {
				return true
			}
		}

		return false
	}

	valMatched := func(test int) bool {
		for _, val := range finalMap {
			if val == test {
				return true
			}
		}

		return false
	}

	for len(finalMap) < len(ruleMatches) {
		for ruleName, ruleVals := range ruleMatches {
			if ruleMatched(ruleName) {
				continue
			}

			unmatchedVals := 0
			unmatchedValIdx := 0
			for idx, val := range ruleVals {
				if !valMatched(val) {
					unmatchedVals++
					unmatchedValIdx = idx
				}
			}
			if unmatchedVals == 1 {
				finalMap[ruleName] = ruleVals[unmatchedValIdx]
				continue
			}

			for _, val := range ruleVals {
				if valMatched(val) {
					continue
				}

				numMatches := 0
				for checkRule, checkVals := range ruleMatches {
					if ruleMatched(checkRule) {
						continue
					}

					for _, checkVal := range checkVals {
						if checkVal == val {
							numMatches++
							break
						}
					}
				}

				if numMatches == 1 {
					finalMap[ruleName] = val
					break
				}
			}
		}
	}

	ruleForColumn := func(col int) string {
		for name, checkVal := range finalMap {
			if col == checkVal {
				return name
			}
		}

		panic(fmt.Sprintf("no rule found for column %d", col))
	}

	finalVal := 1
	for idx, val := range myTicket {
		rule := ruleForColumn(idx)
		if strings.HasPrefix(rule, "departure ") {
			finalVal *= val
		}
	}

	log.Printf("Q16part2: departures multiplied=%d\n", finalVal)
}
