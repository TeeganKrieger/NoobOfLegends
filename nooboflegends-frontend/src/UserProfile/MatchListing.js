import React, { Component } from 'react';
import './MatchListing.css';
import GetPositionIcon from '../Helpers/PositionIconHelper'
import GetChampionIcon from '../Helpers/ChampionHelper'

export default class MatchListing extends Component {

    constructor(props) {
        super(props);
        this.state = { match: props.match, select: props.select, deselect: props.deselect };
    }

    toggleSelect() {
        let match = this.state.match;
        let select = this.state.select;
        let deselect = this.state.deselect;

        let b = document.getElementById(match.id).classList.toggle("selected");
        if (b) {
            document.getElementById(match.id).style.borderLeft = "0.5rem solid " + match.color;
            select(match);
        }
        else {
            document.getElementById(match.id).style.borderLeft = "";
            deselect(match);
        }
    }

    unixTimeToDate(timestamp) {
        let date = new Date(timestamp * 1000);
        let day = date.getDate();
        let month = date.getMonth() + 1;
        let year = date.getFullYear();
        let hours = date.getHours();
        let minutes = date.getMinutes();

        return month + "/" + day + "/" + year + " " + hours + ":" + minutes;
    }

    render() {
        let match = this.state.match;

        return (
            <button id={match.id} className={"listing-box row " + (match.won ? "victory" : "defeat")} onClick={() => this.toggleSelect()}>
                <div className='col-no-padding col-3 align-items-center justify-content-center'>
                    <img className='champion-icon' src={GetChampionIcon(match.champion)} />
                </div>
                <div className='col-no-padding col-3'>
                    <div className={match.won ? "won-span" : "lost-span"}>{match.won ? "Victory" : "Defeat"}</div>
                    <div className='played-on-span'>{this.unixTimeToDate(match.playedOn)}</div>
                </div>
                <div className='col-no-padding col-6'>
                    <div className='kda-title-span'>K/D/A</div>
                    <div className='kda-span'>{match.kills}/{match.deaths}/{match.assists}</div>
                </div>
            </button>
            );
    }
}