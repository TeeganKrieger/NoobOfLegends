import React from 'react';

import unrankedIcon from '../Resources/RankedIcons/Emblem_Unranked.png';
import ironIcon from '../Resources/RankedIcons/Emblem_Iron.png';
import bronzeIcon from '../Resources/RankedIcons/Emblem_Bronze.png';
import silverIcon from '../Resources/RankedIcons/Emblem_Silver.png';
import goldIcon from '../Resources/RankedIcons/Emblem_Gold.png';
import platinumIcon from '../Resources/RankedIcons/Emblem_Platinum.png';
import diamondIcon from '../Resources/RankedIcons/Emblem_Diamond.png';
import masterIcon from '../Resources/RankedIcons/Emblem_Master.png';
import grandmasterIcon from '../Resources/RankedIcons/Emblem_Grandmaster.png';
import challengerIcon from '../Resources/RankedIcons/Emblem_Challenger.png';
import loadingIcon from '../Resources/MiscIcons/Loading.svg'

export default {

    /* Get the icon associated with a specific rank */
     GetRankedIcon(rank) {
        switch (rank) {
            case -2:
                return loadingIcon;
            case -1:
                return unrankedIcon;
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
    },
    /* Get the full proper name associated with a specific rank */
    GetRankedName(rank) {
        switch (rank) {
            case -2:
                return "Loading...";
            case -1:
                return "Unranked";
            case 1:
                return "Iron";
            case 2:
                return "Bronze";
            case 3:
                return "Silver";
            case 4:
                return "Gold";
            case 5:
                return "Platinum";
            case 6:
                return "Diamond";
            case 7:
                return "Master";
            case 8:
                return "GrandMaster";
            case 9:
                return "Challenger";
        }
    },
    /* Get the name associated with a specific tier */
    GetRankedTierName(tier) {
        switch (tier) {
            case -1:
                return "";
            case 1:
                return "I";
            case 2:
                return "II";
            case 3:
                return "III";
            case 4:
                return "IV";
        }
    }
}