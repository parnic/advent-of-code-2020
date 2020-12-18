const fs = require('fs')

let list = []

function deepCopy(obj) {
    if (typeof obj === 'object') {
        if (Array.isArray(obj)) {
            let l = obj.length;
            let r = new Array(l);
            for (let i = 0; i < l; i++) {
                r[i] = deepCopy(obj[i]);
            }
            return r;
        } else {
            let r = {};
            r.prototype = obj.prototype;
            for (let k in obj) {
                r[k] = deepCopy(obj[k]);
            }
            return r;
        }
    }
    return obj;
}

function q17() {
    let start = new Date()
    makeList()
    console.log(`makeList took ${(new Date() - start)}ms`)
    start = new Date()
    part1()
    console.log(`q17part1 took ${(new Date() - start)}ms`)
    start = new Date()
    part2()
    console.log(`q17part2 took ${(new Date() - start)}ms`)
}

function makeList() {
    const lines = fs.readFileSync('17input.txt').toString().split('\n')

    for (let i = 0; i < lines.length; i++) {
        for (let j = 0; j < lines[i].trim().length; j++) {
            if (!list[i]) {
                list[i] = []
            }

            list[i][j] = lines[i][j]
        }
    }
}

function activeNeighbors(arr, initialX, initialY, initialZ) {
    let numActive = 0

    for (let z = Math.max(initialZ - 1, 0); z <= Math.min(initialZ + 1, arr.length - 1); z++) {
        for (let x = Math.max(initialX - 1, 0); x <= Math.min(initialX + 1, arr[z].length - 1); x++) {
            for (let y = Math.max(initialY - 1, 0); y <= Math.min(initialY + 1, arr[z][x].length - 1); y++) {
                if (z === initialZ && x === initialX && y === initialY) {
                    continue
                }

                if (arr[z][x][y] === '#') {
                    numActive++
                }
            }
        }
    }

    return numActive
}

function activeNeighbors4(arr, initialX, initialY, initialZ, initialW) {
    let numActive = 0

    for (let w = Math.max(initialW - 1, 0); w <= Math.min(initialW + 1, arr.length - 1); w++) {
        for (let z = Math.max(initialZ - 1, 0); z <= Math.min(initialZ + 1, arr[w].length - 1); z++) {
            for (let x = Math.max(initialX - 1, 0); x <= Math.min(initialX + 1, arr[w][z].length - 1); x++) {
                for (let y = Math.max(initialY - 1, 0); y <= Math.min(initialY + 1, arr[w][z][x].length - 1); y++) {
                    if (w === initialW && z === initialZ && x === initialX && y === initialY) {
                        continue
                    }

                    if (arr[w][z][x][y] === '#') {
                        numActive++
                    }
                }
            }
        }
    }

    return numActive
}

function expandArea(arr) {
    let xLen = arr[0].length
    let yLen = arr[0][0].length
    let emptyX = Array.from('.'.repeat(yLen + 2))
    let emptyZ = Array.from({ length: xLen + 2 }, () => emptyX)

    for (let z = 0; z < arr.length; z++) {
        for (let x = 0; x < arr[z].length; x++) {
            arr[z][x].unshift('.')
            arr[z][x].push('.')
        }

        arr[z].unshift(emptyX)
        arr[z].push(emptyX)
    }

    arr.unshift(emptyZ)
    arr.push(emptyZ)
}

function expandArea4(arr) {
    let zLen = arr[0].length

    for (let w = 0; w < arr.length; w++) {
        expandArea(arr[w])
    }

    let emptyW = Array.from({ length: zLen + 2 }, () => arr[0][0])

    arr.unshift(emptyW)
    arr.push(emptyW)
}

function part1() {
    let current = []
    current[0] = deepCopy(list)

    for (let loops = 0; loops < 6; loops++) {
        expandArea(current)
        let copy = deepCopy(current)

        for (let z = 0; z < current.length; z++) {
            for (let x = 0; x < current[z].length; x++) {
                for (let y = 0; y < current[z][x].length; y++) {
                    let numActive = activeNeighbors(current, x, y, z)
                    if (current[z][x][y] === '#') {
                        if (numActive !== 2 && numActive !== 3) {
                            copy[z][x][y] = '.'
                        }
                    } else {
                        if (numActive === 3) {
                            copy[z][x][y] = '#'
                        }
                    }
                }
            }
        }

        current = deepCopy(copy)
    }

    console.log(`q17part1: answer=${current.flat(Infinity).filter(i => i === '#').length}`)
}

function part2() {
    let current = []
    current[0] = []
    current[0][0] = deepCopy(list)

    for (let loops = 0; loops < 6; loops++) {
        expandArea4(current)
        let copy = deepCopy(current)

        for (let w = 0; w < current.length; w++) {
            for (let z = 0; z < current[w].length; z++) {
                for (let x = 0; x < current[w][z].length; x++) {
                    for (let y = 0; y < current[w][z][x].length; y++) {
                        let numActive = activeNeighbors4(current, x, y, z, w)
                        if (current[w][z][x][y] === '#') {
                            if (numActive !== 2 && numActive !== 3) {
                                copy[w][z][x][y] = '.'
                            }
                        } else {
                            if (numActive === 3) {
                                copy[w][z][x][y] = '#'
                            }
                        }
                    }
                }
            }
        }

        current = deepCopy(copy)
    }

    console.log(`q17part2: answer=${current.flat(Infinity).filter(i => i === '#').length}`)
}

q17()