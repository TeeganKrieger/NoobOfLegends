//Code provided by: https://stackoverflow.com/questions/10014271/generate-random-color-distinguishable-to-humans

import React from 'react';

function selectColor(colorNum, colors) {
    if (colors < 1) colors = 1;
    return "hsl(" + (colorNum * (360 / colors) % 360) + ",100%,50%)";
}

function range(start, end) {
    return Array(end - start + 1).fill().map((_, idx) => start + idx)
}

/* Get a set of *count* distinct colors */
export default function GetColorSet(count) {

    let indices = range(0, count - 1);
    indices.sort(() => (Math.random() > .5) ? 1 : -1);

    let colors = [];

    for (let i = 0; i < count; i++) {
        colors.push(selectColor(indices[i], count));
    }

    return colors;
}