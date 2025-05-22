import styled from '@emotion/styled';
import { Box, Typography } from "@mui/material";

const OptionContainer = styled.div`
    display: flex;
    flex-direction: row;
    padding: 4px 12px;
    margin-bottom: 12px;
`;

const TextContainer = styled(Box)`
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%
`;

const Suggestion = ({ suggestion }) => {
    const { id, value, slug, year } = suggestion;
    return (
        <a href={`/games/${slug}`} key={id}>
            <OptionContainer>
                <TextContainer>
                    <Typography comonent="span" variant="body1">
                        {value} ({year})
                    </Typography>
                </TextContainer>
            </OptionContainer>
        </a>
    );
};

export default Suggestion;