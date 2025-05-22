import { Chart as ChartJS, CategoryScale, LinearScale, BarElement, Tooltip } from 'chart.js';
import { Bar } from 'react-chartjs-2';

ChartJS.register(CategoryScale, LinearScale, BarElement, Tooltip);

const ReviewChart = ({ reviewScores }) => {

    const labels = ['0.5★', '1★', '1.5★', '2★', '2.5★', '3★', '3.5★', '4★', '4.5★', '5★'];

    const data = {
        labels: labels,
        datasets: [
            {
                data: reviewScores,
                backgroundColor: '#fc6399'
            }
        ]
    };

    const options = {
        responsive: true,
        scales: {
            x: {
                border: {
                    color: 'rgba(162,177,229,.2)'
                },
                grid: {
                    display: false
                },
                max: 5,
                ticks: {
                    autoSkip: true
                }
            },
            y: {
                display: false,
                grid: {
                    display: true
                },
                min: 0,
                ticks: {
                    autoSkip: true
                }
            }
        }
    };

    return (
        <div className="graph-container">
            <Bar data={data} options={options} />
        </div>
    );
};

export default ReviewChart;