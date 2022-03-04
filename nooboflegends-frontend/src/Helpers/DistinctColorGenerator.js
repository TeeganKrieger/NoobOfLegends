import React from 'react';

function selectColor(colorNum, colors) {
    if (colors < 1) colors = 1; // defaults to one color - avoid divide by zero
    return "hsl(" + (colorNum * (360 / colors) % 360) + ",100%,50%)";
}

function range(start, end) {
    return Array(end - start + 1).fill().map((_, idx) => start + idx)
}

export default function GetColorSet(count) {

    let indices = range(0, count - 1);
    indices.sort(() => (Math.random() > .5) ? 1 : -1);

    let colors = [];

    for (let i = 0; i < count; i++) {
        colors.push(selectColor(indices[i], count));
    }

    return colors;
}