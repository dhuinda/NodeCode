import { FC } from 'react'
import { Flex, Input, Text } from '@chakra-ui/react'

const NavBar: FC = () => {
  return (
    <Flex alignItems='center' justifyContent='space-between'>
      <Text fontFamily='Teko' fontSize='48px'>cdpm</Text>
      <Input w='30vw' borderColor='grayBorder' _hover={{ borderColor: 'brightBlue'}} _selected={{ borderColor: 'brightBlue' }} _active={{ borderColor: 'brightBlue' }} />
    </Flex>
  )
}

export default NavBar
