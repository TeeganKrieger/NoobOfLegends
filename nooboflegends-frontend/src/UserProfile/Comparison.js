import React, { Component } from 'react';
import './Comparison.css';
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    BarElement,
    Title,
    Tooltip,
    Legend,
} from 'chart.js';
import { Bar } from 'react-chartjs-2';
import GetColorSet from '../Helpers/DistinctColorGenerator'

export default class Comparison extends Component {

    constructor(props) {
        super(props);
        this.state = { name: props.name, matches: props.matches, lambda: props.lambda };

        ChartJS.register(
            CategoryScale,
            LinearScale,
            BarElement,
            Title,
            Tooltip,
            Legend
        );
    }

    componentWillReceiveProps(props) {
        this.state = { name: props.name, matches: props.matches, lambda: props.lambda };
    }

    render() {
        let name = this.state.name;
        let matches = this.state.matches;
        let lambda = this.state.lambda;

        let vals = [];
        let labels = [];

        for (let i = 0; i < matches.length; i++) {
            let val = lambda(matches[i]);
            vals.push(val);
            labels.push("Match " + (i + 1));
        }

        let options = {
            indexAxis: 'y',
            maintainAspectRatio: false,
            elements: {
                bar: {
                    borderWidth: 2,
                },
            },
            plugins: {
                title: {
                    display: true,
                    text: name
                },
                legend: {
                    display: false,
                }
            },
            scales: {
                yAxes: {
                    display: false,
                }
            }
        };

        let data = {
            labels: labels,
            datasets: [
                {
                    label: name,
                    data: vals,
                    borderColor: 'rgb(0, 0, 0)',
                    backgroundColor: GetColorSet(vals.length),
                },
            ],
        };

        let height = Math.max(150, 33.33 * vals.length);

        return (
            <div className='chart-container'>
                <Bar height={height} options={options} data={data} />
            </div>
        );
    }
}