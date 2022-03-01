import React from 'react';

import ironIcon from '../Resources/RankedIcons/Emblem_Iron.png';
import bronzeIcon from '../Resources/RankedIcons/Emblem_Bronze.png';
import silverIcon from '../Resources/RankedIcons/Emblem_Silver.png';
import goldIcon from '../Resources/RankedIcons/Emblem_Gold.png';
import platinumIcon from '../Resources/RankedIcons/Emblem_Platinum.png';
import diamondIcon from '../Resources/RankedIcons/Emblem_Diamond.png';
import masterIcon from '../Resources/RankedIcons/Emblem_Master.png';
import grandmasterIcon from '../Resources/RankedIcons/Emblem_Grandmaster.png';
import challengerIcon from '../Resources/RankedIcons/Emblem_Challenger.png';

export default function GetRankedIcon(rank) {

    switch (rank) {
        case 1:
            return ironIcon;
        case 2:
            return bronzeIcon;
        case 3:
            return silverIcon;
        case 4:
            return goldIcon;
        case 5:
            return platinumIcon;
        case 6:
            return diamondIcon;
        case 7:
            return masterIcon;
        case 8:
            return grandmasterIcon;
        case 9:
            return challengerIcon;
    }
}
