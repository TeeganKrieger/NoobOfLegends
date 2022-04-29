//Original Code provided by: https://stackoverflow.com/questions/10014271/generate-random-color-distinguishable-to-humans

import React from 'react';

/**
 * Converts a javascript color into a css color.
 * @param {any} colorNum The index of the color.
 * @param {any} colors The array of all colors.
 */
function selectColor(colorNum, colors) {
    if (colors < 1) colors = 1;
    return "hsl(" + (colorNum * (360 / colors) % 360) + ",100%,50%)";
}

/**
 * Function that attempts to mimic the Python range function.
 * @param {any} start The number to begin the range at.
 * @param {any} end The number to end the range at. (Inclusive)
 */
function range(start, end) {
    return Array(end - start + 1).fill().map((_, idx) => start + idx)
}

/**
 * Gets *count* number of distinct colors.
 * @param {any} username The username of the user needing these colors. This is used to seed the color randomizer.
 * @param {any} count The number of colors to generate.
 */
export default function GetColorSet(username, count) {

    var seed = xmur3(username);
    var rand = mulberry32(seed());

    let indices = range(0, count - 1);
    indices.sort(() => (rand() > .5) ? 1 : -1);

    let colors = [];

    for (let i = 0; i < count; i++) {
        colors.push(selectColor(indices[i], count));
    }

    return colors;
}

/* Our seeded random implementations */
/* All algos provided by: https://stackoverflow.com/questions/521295/seeding-the-random-number-generator-in-javascript */

/**
 * Generates an expression used to generate a hash from a string.
 * @param {any} str The string to hash.
 */
function xmur3(str) {
    for (var i = 0, h = 1779033703 ^ str.length; i < str.length; i++) {
        h = Math.imul(h ^ str.charCodeAt(i), 3432918353);
        h = h << 13 | h >>> 19;
    } return function () {
        h = Math.imul(h ^ (h >>> 16), 2246822507);
        h = Math.imul(h ^ (h >>> 13), 3266489909);
        return (h ^= h >>> 16) >>> 0;
    }
}

/**
 * Generates an expressions that generates seeded random numbers.
 * @param {any} a The seed to use.
 */
function mulberry32(a) {
    return function () {
        var t = a += 0x6D2B79F5;
        t = Math.imul(t ^ t >>> 15, t | 1);
        t ^= t + Math.imul(t ^ t >>> 7, t | 61);
        return ((t ^ t >>> 14) >>> 0) / 4294967296;
    }
}