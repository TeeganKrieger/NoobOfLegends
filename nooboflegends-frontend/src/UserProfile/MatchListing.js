import React, { Component } from 'react';
import './MatchListing.css';
import GetRankedIcon from '../Helpers/RankedIconHelper'
import GetPositionIcon from '../Helpers/PositionIconHelper'
import GetChampionIcon from '../Helpers/ChampionHelper'

export default class MatchListing extends Component {

    constructor(props) {
        super(props);
        this.state = {match: props.match};
    }

    render() {
        let match = this.state.match;
        return (
            <div className={"listing-box row " + (match.won ? "victory" : "defeat")}>
                <div className='col-3 my-auto'>
                    <img className='champion-icon' src={GetChampionIcon(match.champion)} />
                </div>
                <div className='col-4'>
                    <div className='won-span'>{match.won ? "Victory" : "Defeat"}</div>
                    <div className='played-on-span'>Played on: {match.playedOn}</div>
                </div>
                <div className='col-5'>
                    <div className='kda-title-span'>K/D/A</div>
                    <div className='kda-span'>{match.kills}/{match.deaths}/{match.assists}</div>
                </div>
            </div>
            );
    }
}