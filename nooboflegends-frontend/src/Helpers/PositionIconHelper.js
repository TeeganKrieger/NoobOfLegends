import React from 'react';

import topIcon from '../Resources/PositionIcons/Top.png';
import midIcon from '../Resources/PositionIcons/Mid.png';
import botIcon from '../Resources/PositionIcons/Bot.png';
import supportIcon from '../Resources/PositionIcons/Support.png';
import jungleIcon from '../Resources/PositionIcons/Jungle.png';

/**
 * Get the icon for a position.
 * @param {any} position The ID of the position to get the icon of.
 */
export default function GetPositionIcon(position) {

    switch (position) {
        case "TOP":
            return topIcon;
        case "MID":
            return midIcon;
        case "BOT":
            return botIcon;
        case "SUPPORT":
            return supportIcon;
        case "JUNGLE":
            return jungleIcon;
    }
}