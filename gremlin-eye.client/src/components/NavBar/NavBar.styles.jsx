import styled from '@emotion/styled';
import { AppBar, Toolbar, Menu } from '@mui/material';

export const NavbarContainer = styled(AppBar)`
    align-items: center;
    background-color: transparent;
    border: none;
    box-sizing: border-box;
    display: flex
    flex-flow: row nowrap;
    justify-content: flex-start;
    padding: .5rem 1rem;
    position: relative;
    width: 100%;
`;

export const NavbarContent = styled(Toolbar)`
    display: flex;
    height: 50px;
    max-width: 1152px;
    min-height: unset;
    width: 100%;
`;

export const DropdownMenu = styled(Menu)(({ theme }) => ({
    '& .MuiPaper-root': {
        backgroundColor: '#4a5e8d',
        border: '0',
        borderRadius: '4px',
        boxShadow: '0 .4rem .5rem 0 rgba(27,29,41,.52)',
        minWidth: 0,
        position: 'absolute'
    }
}));