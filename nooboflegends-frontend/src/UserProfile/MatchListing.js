import React, { Component } from 'react';
import './MatchListing.css';
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
            <button className={"listing-box row selected " + (match.won ? "victory" : "defeat")}>
                <div className='col-no-padding col-3 align-items-center justify-content-center'>
                    <img className='champion-icon' src={GetChampionIcon(match.champion)} />
                </div>
                <div className='col-no-padding col-3'>
                    <div className={match.won ? "won-span" : "lost-span"}>{match.won ? "Victory" : "Defeat"}</div>
                    <div className='played-on-span'>Played on: {match.playedOn}</div>
                </div>
                <div className='col-no-padding col-6'>
                    <div className='kda-title-span'>K/D/A</div>
                    <div className='kda-span'>{match.kills}/{match.deaths}/{match.assists}</div>
                </div>
            </button>
            );
    }
}